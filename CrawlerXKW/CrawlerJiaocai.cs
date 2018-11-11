using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.Common;
using Grabzujuan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSoup;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace CrawlerXKW
{
    public class CrawlerJiaocai
    {
        public void Crawler()
        {

            foreach (var category in GetSubjectCategories())
            {
                var res = HttpWebRequestUtility.Post("http://zujuan.xkw.com/xxyw/zj108223/?action=getcategory", $"cid={category.CategoryId}");

                var doc = NSoupClient.Parse(res.ToString());

                var a = doc.GetElementsByTag("a");

                foreach (var item in a)
                {
                    var jiaocaidId = item.Attr("gradeid").NullToInt();
                    var jiaocaidName = item.Attr("title").NullToString();
                    var url = $"http://zujuan.xkw.com/{item.Attr("href").NullToString()}";
                    AddJiaocai(category.CategoryId, jiaocaidId, jiaocaidName, url);
                }
            }

        }

        public List<SubjectCategory> GetSubjectCategories()
        {
            using (var db = new XKWEntities2())
            {
                return db.SubjectCategory.Where(t => !t.CategoryName.Contains("综合库")).ToList();
            }
        }

        public void AddJiaocai(int categoryId, int jiaocaiId, string jiaocaiName, string jiaocaiUrl)
        {
            using (var db = new XKWEntities2())
            {
                if (db.JiaoCai.Any(t => t.JiaoCaiId == jiaocaiId))
                    return;
                var entity = new JiaoCai();
                entity.CategoryId = categoryId;
                entity.JiaoCaiId = jiaocaiId;
                entity.JCName = jiaocaiName;
                entity.JiaoCaiUrl = jiaocaiUrl;
                db.JiaoCai.Add(entity);
                db.SaveChanges();
            }

        }

        public void AddArea(int areaId, string name, string shortName)
        {
            using (var db = new XKWEntities2())
            {
                if (!db.Area.Any(t => t.AreaId == areaId))
                {
                    var entity = new Area();
                    entity.AreaId = areaId;
                    entity.Name = name;
                    entity.ShortName = shortName;
                    db.Area.Add(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
