using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabzujuan.Common;
using java.net;
using Newtonsoft.Json.Linq;
using NSoup;

namespace Grabzujuan
{
    public class CategoryCrawler
    {
        //public void InitBook()
        //{
        //    Parallel.ForEach(typeof(ChildEnum).GetEnumSource(), (child) =>
        //   {
        //       Parallel.ForEach(typeof(DegreeEnum).GetEnumSource(), (degree) =>
        //       {



        //       });
        //   });
        //    foreach (var child in typeof(ChildEnum).GetEnumSource())
        //    {
        //        foreach (var degree in typeof(DegreeEnum).GetEnumSource())
        //        {
        //            var childId = child.Item1.NullToInt();
        //            var degreeId = degree.Item1.NullToInt();
        //            var url = string.Format("https://www.zujuan.com/question?chid={0}&xd={1}", child.Item1, degree.Item1);

        //            var html = new HttpUnitHelper().GetRealHtmlTrice(url);

        //            var doc = NSoupClient.Parse(html);

        //            //获取当前dgree下的科目下的教材版本
        //            var bookTypeDoc = doc.Select("div.search-type div.con-items")[0].GetElementsByTag("a");

        //            foreach (var element in bookTypeDoc)
        //            {
        //                var elementId = element.Attr("data-bcaid");
        //                var name = element.Text();

        //                DataService.AddBook(childId, degreeId, name, elementId.NullToInt());
        //            }
        //        }
        //    }
        //}

        //public void InitCategory()
        //{
        //    var listBook = books.GetBooklist();
        //    foreach (var book in listBook)
        //    {
        //        var url = string.Format("https://www.zujuan.com/question?bookversion={0}&chid={1}&xd={2}",
        //            book.BookVersionId, book.Child, book.Degree);

        //        var html = new HttpUnitHelper().GetRealHtmlTrice(url);
        //        var doc = NSoupClient.Parse(html);

        //        //获取当前dgree下的科目下的教材版本
        //        var categoryDoc = doc.Select("div.search-type div.con-items")[1].GetElementsByTag("a");
        //        //
        //        var total = doc.Select("div.total b")[0].Text().NullToInt();
        //        foreach (var element in categoryDoc)
        //        {
        //            var elementId = element.Attr("data-bcaid");
        //            var name = element.Text();

        //            DataService.AddCategory(book.Id, elementId.NullToInt(), name, total);
        //        }
        //    }
        //    //https://www.zujuan.com/question?chid=2&xd=1
        //}

       
    }
}
