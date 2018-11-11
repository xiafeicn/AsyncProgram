using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public class CrawlerJiaocaiDetail
    {
        public void Crawler()
        {
            var listJaiCai = GetJiaoCais();
            Parallel.ForEach(listJaiCai, (jiaocai) =>
            {
                Recrusion(jiaocai.JiaoCaiId, jiaocai.JiaoCaiId);
            });
        }

        public void Recrusion(int jiaocaiId, int id)
        {
            var url = $"http://zujuan.xkw.com/Web/Handler1.ashx?action=categorytreewithchild&parentid={id}&iszsd=0";
            var result = HttpWebResponseUtility.ExecuteCreateGetHttpResponse(url, 10000,  null);
            if (string.IsNullOrEmpty(result))
                return;
            var doc = NSoupClient.Parse(result);
            var elements = doc.Select("body>ul>li");

            foreach (var element in elements)
            {
                var currentId = element.Attr("id").NullToInt();
                var currentText = element.Select(">a").Text;
                //add
                AddJiaocaiDetail(jiaocaiId, id, currentId, currentText);


                Recrusion(jiaocaiId, currentId);
            }
        }

        public List<JiaoCai> GetJiaoCais()
        {
            using (var db = new XKWEntities2())
            {
                return db.JiaoCai.ToList();
            }
        }
        public void AddJiaocaiDetail(int jiaocaiId, int parentId, int id, string name)
        {
            using (var db = new XKWEntities2())
            {
                if (db.JiaocaiDetail.Any(t => t.JiaoCaiDetailId == id))
                {
                    return;
                }
                var entity = new JiaocaiDetail();
                entity.JiaoCaiId = jiaocaiId;
                entity.JiaoCaiDetailParentId = parentId;
                entity.JiaoCaiDetailId = id;
                entity.JiaoCaiDetailName = name;
                db.JiaocaiDetail.Add(entity);
                db.SaveChanges();
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
