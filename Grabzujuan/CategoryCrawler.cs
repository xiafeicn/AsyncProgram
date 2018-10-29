using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabzujuan.Common;
using NSoup;

namespace Grabzujuan
{
    public class CategoryCrawler
    {
        public void InitCategory()
        {
            foreach (var child in typeof(ChildEnum).GetEnumSource())
            {
                foreach (var degree in typeof(DegreeEnum).GetEnumSource())
                {
                    var url = string.Format("https://www.zujuan.com/question?chid={0}&xd={1}", child.Item1, degree.Item1);

                    var html = new HttpUnitHelper().GetRealHtmlTrice(url);

                    var doc = NSoupClient.Parse(html);

                    //获取当前dgree下的科目下的教材版本
                    var bookTypeDoc = doc.Select("div.search-type.con-items")[0].GetElementsByTag("a");


                    var categoryDoc= doc.Select("div.search-type.con-items")[0].GetElementsByTag("a");
                    break;
                }
            }

            //https://www.zujuan.com/question?chid=2&xd=1
        }
    }
}
