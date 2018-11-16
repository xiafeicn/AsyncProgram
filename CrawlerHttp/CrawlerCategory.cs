using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Grabzujuan;
using Grabzujuan.Common;
using NSoup;

namespace CrawlerHttp
{
    public class CrawlerCategory
    {
        public void InitCategory()
        {
            var listBook = BookService.GetBooklist();
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
        }
    }
}
