using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabzujuan;
using Grabzujuan.Common;
using java.net;
using Newtonsoft.Json.Linq;
using NSoup;

namespace CrawlerHttp
{
    public class CrawlerCategoryUrllist
    {
        /// <summary>
        /// 整站采集，如果整体数据过一遍，需要重新生成这些category下每页的URL、
        /// </summary>58
        public void InitCatePageUrl(int startIndex, int endIndex)
        {
            var listCategory = DataService.GetCategorylist();

            //https://www.zujuan.com/question?categories=47854&bookversion=47832&nianji=47854&chid=3&xd=1
            foreach (var category in listCategory)

            //Parallel.ForEach(listCategory, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, (category) =>
            {
                var index = listCategory.IndexOf(category);
                if (index <= endIndex && index >= startIndex)
                {


                    Debug.WriteLine($"start index {index}   total:{listCategory.Count}");
                    Console.WriteLine($"start index {index}   total:{listCategory.Count}");
                    //if (hasCateID.Any(t => t == category.CategoryId))
                    //{
                    //    continue;
                    //}
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    var api =
                       $"https://www.zujuan.com/question/list?categories={category.CategoryId}&sortField=time&page=1&xd={category.Degree}&chid={category.Child}&_=1540841532659";

                    var url = $"https://www.zujuan.com/question?categories={category.CategoryId}&bookversion={category.BookVersionId}&nianji={category.CategoryId}&chid={category.Child}&xd={category.Degree}";
                    var html = new HttpUnitHelper().GetRealHtmlOnceNotWaitJs(url);
                    var questionCount = NSoupClient.Parse(html).GetElementById("J_QuestionList").GetElementsByTag("li").Count;
                    if (questionCount <= 0) continue;
                    long start = 1000000000000;
                    long end = 1540849999999;
                    long randomId = 1540840000000 + new Random().Next(0000000, 9999999);
                    ////todo 题型筛选

                    var cookies = new HttpUnitHelper().webClient.GetCookies(new URL("https://www.zujuan.com/"));
                    var headerCookie = string.Empty;
                    foreach (var cookie in cookies)
                    {
                        headerCookie += $"{cookie.Name}={cookie.Value};";
                    }
                    //var cookie = DataService.GetCookieState(category.Child, category.Degree);

                    var json = HttpClientHolder.Execute(api, headerCookie);

                    var total = JObject.Parse(json);
                    var totalCount = total["total"].NullToInt();
                    if (totalCount <= 0) throw new Exception("wronng!");
                    var pageNum = totalCount / 10 + 1;
                    DataService.UpdateCategoryTotalCount(category.CategoryId, totalCount);

                    try
                    {
                        Parallel.For(1, pageNum, (i) =>
                        {
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

                            if (!DataService.IsCateUrlGrabed(category.CategoryId, i))
                            {
                                var currentApi =
                                    $"https://www.zujuan.com/question/list?categories={category.CategoryId}&sortField=time&page={i}&_={randomId}";
                                var currentJson = HttpClientHolder.Execute(currentApi, headerCookie);
                                Console.WriteLine($"{currentApi}");

                                DataService.AddCateUrl(grabUrl, category.CategoryId, i, currentApi, currentJson, index);
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }

                    sw.Stop();

                    Debug.WriteLine("cost" + sw.ElapsedMilliseconds);

                }
            }
            //);
        }
    }
}
