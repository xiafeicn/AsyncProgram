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
    public class CrawlerQuestionJiaoCaiPageList
    {
        public void Crawler()
        {
            while (ExistCrawlerJC())
            {
                var listJiaocai = GetRandom10CrawlerJC();
                var listArea = GetAreas();
                Parallel.ForEach(listJiaocai, (jc) =>
                {
                    try
                    {
                            Parallel.ForEach(listArea, (area) =>
                            {
                                if (ExistGrabPageAreaSource(area.AreaId, jc.JiaoCaiId))
                                    return;
                                var url =
                                    $"http://zujuan.xkw.com/{jc.Prefix}/zj{jc.JiaoCaiId}/a{area.AreaId}/";


                                Console.WriteLine(url);
                                var html = HttpWebResponseUtility.ExecuteCreateGetHttpResponseProxy(url, 3000, null);

                                var doc = NSoupClient.Parse(html);
                                var totalCount = doc.GetElementById("questioncount").Text().NullToInt();

                                var pageCount = totalCount / 10 + 1;

                                AddGrabPageSource(area.AreaId, jc.JiaoCaiId, totalCount);
                                //for (int i = 1; i <= pageCount; i++)
                                //{
                                //    AddGrabPageList(area.AreaId, jc.JiaoCaiDetailId, grade.GradeId, totalCount, i);
                                //}
                            });

                        UpdateJiaocaiStatus(jc.JiaoCaiId);
                    }
                    catch
                    {

                    }
                });
            }

        }


        public void UpdateJiaocaiStatus(int jiaocaiId)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.JiaoCai.Any(t => t.JiaoCaiId == jiaocaiId))
                {
                    var entity = db.JiaoCai.FirstOrDefault(t => t.JiaoCaiId == jiaocaiId);
                    entity.Status = true;
                    db.SaveChanges();
                }
            }
        }

        public bool ExistGrabPageAreaSource(int areaId, int jiaocaiId)
        {
            using (var db = new XKWEntities2())
            {
                return db.QuestionJiaoCaiSource.Any(
                    t => t.AreaId == areaId && t.JiaocaiId== jiaocaiId);
            }
        }

        public void AddGrabPageSource(int areaId, int jiaocaiId, int total)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.QuestionJiaoCaiSource.Any(
                        t => t.AreaId == areaId && t.JiaocaiId == jiaocaiId))
                    return;
                var entity = new QuestionJiaoCaiSource();
                entity.AreaId = areaId;
                entity.JiaocaiId = jiaocaiId;
                entity.Total = total;
                db.QuestionJiaoCaiSource.Add(entity);
                db.SaveChanges();
            }
        }

        public void AddGrabPageList(int areaId, int jiaocaiDetailId, int gradeId, int total, int pageNum)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.QuestionPageList.Any(
                        t => t.AreaId == areaId && t.JiaocaiDetailId == jiaocaiDetailId && t.GradeId == gradeId && t.PageNum == pageNum))
                    return;
                var entity = new QuestionPageList();
                entity.AreaId = areaId;
                entity.JiaocaiDetailId = jiaocaiDetailId;
                entity.GradeId = gradeId;
                entity.Total = total;
                entity.PageNum = pageNum;
                db.QuestionPageList.Add(entity);
                db.SaveChanges();
            }
        }
        public bool ExistCrawlerJC()
        {
            using (var db = new XKWEntities2())
            {
                return db.V_Jiaocai.Any(t => t.Status == false);
            }
        }
        public List<V_Jiaocai> GetRandom10CrawlerJC()
        {
            using (var db = new XKWEntities2())
            {

                return
                    db.Database.SqlQuery<V_Jiaocai>(
                        @"
select top 10 *, NewID() as random from [V_Jiaocai] where status=0 order by random").ToList();
            }
        }
        public List<SubjectGrade> GetSubjectGrade()
        {
            using (var db = new XKWEntities2())
            {
                return db.SubjectGrade.ToList();
            }
        }
        public List<Area> GetAreas()
        {
            using (var db = new XKWEntities2())
            {
                return db.Area.ToList();
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
