using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Grabzujuan;
using Grabzujuan.Common;
using NSoup;

namespace CrawlerHttp
{
    public class CrawlerBook
    {
        public void InitBook()
        {
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

                        Console.WriteLine("add book");
                        BookService.AddBook(childId, degreeId, name, elementId.NullToInt());
                    }
                }
            }
        }
    }
}
