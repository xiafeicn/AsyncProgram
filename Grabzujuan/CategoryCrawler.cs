using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabzujuan.Common;
using Grabzujuan.Data;
using java.net;
using Newtonsoft.Json.Linq;
using NSoup;

namespace Grabzujuan
{
    public class CategoryCrawler
    {
        public void InitBook()
        {
            Parallel.ForEach(typeof(ChildEnum).GetEnumSource(), (child) =>
           {
               Parallel.ForEach(typeof(DegreeEnum).GetEnumSource(), (degree) =>
               {



               });
           });
            foreach (var child in typeof(ChildEnum).GetEnumSource())
            {
                foreach (var degree in typeof(DegreeEnum).GetEnumSource())
                {
                    var childId = child.Item1.NullToInt();
                    var degreeId = degree.Item1.NullToInt();
                    var url = string.Format("https://www.zujuan.com/question?chid={0}&xd={1}", child.Item1, degree.Item1);

                    var html = new HttpUnitHelper().GetRealHtmlTrice(url);

                    var doc = NSoupClient.Parse(html);

                    //获取当前dgree下的科目下的教材版本
                    var bookTypeDoc = doc.Select("div.search-type div.con-items")[0].GetElementsByTag("a");

                    foreach (var element in bookTypeDoc)
                    {
                        var elementId = element.Attr("data-bcaid");
                        var name = element.Text();

                        DataService.AddBook(childId, degreeId, name, elementId.NullToInt());
                    }
                }
            }
        }

        public void InitCategory()
        {
            var listBook = DataService.GetBooklist();
            foreach (var book in listBook)
            {
                var url = string.Format("https://www.zujuan.com/question?bookversion={0}&chid={1}&xd={2}",
                    book.BookVersionId, book.Child, book.Degree);

                var html = new HttpUnitHelper().GetRealHtmlTrice(url);
                var doc = NSoupClient.Parse(html);

                //获取当前dgree下的科目下的教材版本
                var categoryDoc = doc.Select("div.search-type div.con-items")[1].GetElementsByTag("a");
                //
                var total = doc.Select("div.total b")[0].Text().NullToInt();
                foreach (var element in categoryDoc)
                {
                    var elementId = element.Attr("data-bcaid");
                    var name = element.Text();

                    DataService.AddCategory(book.Id, elementId.NullToInt(), name, total);
                }
            }
            //https://www.zujuan.com/question?chid=2&xd=1
        }

        /// <summary>
        /// 整站采集，如果整体数据过一遍，需要重新生成这些category下每页的URL、
        /// </summary>
        public void InitCatePageUrl()
        {
            var listCategory = DataService.GetCategorylist();

            var hasCateID = DataService.GetCrawleredCatelist();
            //https://www.zujuan.com/question?categories=47854&bookversion=47832&nianji=47854&chid=3&xd=1
            foreach (var category in listCategory)

            //Parallel.ForEach(listCategory, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, (category) =>
            {
                if (hasCateID.Any(t => t == category.CategoryId))
                {
                    continue;
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //通过页面采集方式，放弃
                //var url = $"https://www.zujuan.com/question?categories={category.CategoryId}&bookversion={category.BookVersionId}&nianji={category.CategoryId}&chid={category.Child}&xd={category.Degree}";
                //var html = new HttpUnitHelperNosingleton().GetRealHtmlTrice(url);
                //var doc = NSoupClient.Parse(html);
                //var total = doc.Select("div.total b")[0].Text().NullToInt();

                //DataService.UpdateCategoryCount(category.Id, total);

                //var pageNum = total / 10 + 1;
                //for (int i = 1; i <= pageNum; i++)
                //{
                //    var grabUrl = "";
                //    if (i > 1)
                //    {
                //        grabUrl =
                //            $"https://www.zujuan.com/question/index?categories={category.CategoryId}&bookversion={category.BookVersionId}&nianji=7741&chid={category.Child}&xd={category.Degree}&page={i}&per-page=10";
                //    }
                //    else
                //    {
                //        grabUrl = url;
                //    }

                //    DataService.AddCateUrl(grabUrl, category.CategoryId, i);
                //}
                var api =
                   $"https://www.zujuan.com/question/list?categories={category.CategoryId}&sortField=time&page=1&_=1540841532659";

                var url = $"https://www.zujuan.com/question?categories={category.CategoryId}&bookversion={category.BookVersionId}&nianji={category.CategoryId}&chid={category.Child}&xd={category.Degree}";
                var html = new HttpUnitHelper().GetRealHtmlOnceNotWaitJs(url);

                long start = 1000000000000;
                long end = 1540849999999;
                long randomId = 1540840000000 + new Random().Next(0000000, 9999999);
                //todo 题型筛选
               
                var cookies = new HttpUnitHelper().webClient.GetCookies(new URL("https://www.zujuan.com/"));
                var headerCookie = string.Empty;
                foreach (var cookie in cookies)
                {
                    headerCookie += $"{cookie.Name}={cookie.Value};";
                }

                var json = HttpClientHolder.GetRequest(api, headerCookie);

                var total = JObject.Parse(json);
                var totalCount = total["total"].NullToInt();

                var pageNum = totalCount / 10 + 1;
                Parallel.For(1, pageNum, (i) =>
                {
                    var currentApi =
           $"https://www.zujuan.com/question/list?categories={category.CategoryId}&sortField=time&page={i}&_={randomId}";
                    var currentJson = HttpClientHolder.GetRequest(currentApi, headerCookie);
                    var grabUrl = "";
                    if (i > 1)
                    {
                        grabUrl =
                            $"https://www.zujuan.com/question/index?categories={category.CategoryId}&bookversion={category.BookVersionId}&nianji=7741&chid={category.Child}&xd={category.Degree}&page={i}&per-page=10";
                    }
                    else
                    {
                        grabUrl = url;
                    }

                    DataService.AddCateUrl(grabUrl, category.CategoryId, i, currentApi, currentJson);
                });

                sw.Stop();

                Debug.WriteLine("cost" + sw.ElapsedMilliseconds);

            }
            //);
        }
    }
}
