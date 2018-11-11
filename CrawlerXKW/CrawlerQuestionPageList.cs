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
    public class CrawlerQuestionPageList
    {
        public void Crawler()
        {

            var listSubjectGrade = GetSubjectGrade();
            var listArea = GetAreas();
            while (ExistCrawlerJC())
            {
                var listJiaocai = GetRandom10CrawlerJC();

                Parallel.ForEach(listJiaocai, (jc) =>
                {
                    var grades = listSubjectGrade.Where(t => t.SubjectId == jc.SubjectId);
                    try
                    {
                        Parallel.ForEach(grades, (grade) =>
                        {
                            Parallel.ForEach(listArea, (area) =>
                            {
                                if (ExistGrabPageSource(area.AreaId, jc.JiaoCaiDetailId, grade.GradeId))
                                    return;
                                var url =
                                    $"http://zujuan.xkw.com/{jc.Prefix}/zj{jc.JiaoCaiDetailId}/a{area.AreaId}g{grade.GradeId}/";


                                Console.WriteLine(url);
                                var html = HttpWebResponseUtility.ExecuteCreateGetHttpResponseProxy(url, 3000, null);

                                var doc = NSoupClient.Parse(html);
                                var totalCount = doc.GetElementById("questioncount").Text().NullToInt();

                                var pageCount = totalCount/10 + 1;

                                AddGrabPageSource(area.AreaId, jc.JiaoCaiDetailId, grade.GradeId, totalCount);
                                //for (int i = 1; i <= pageCount; i++)
                                //{
                                //    AddGrabPageList(area.AreaId, jc.JiaoCaiDetailId, grade.GradeId, totalCount, i);
                                //}
                            });
                        });


                        UpdateJiaocaiDetailStatus(jc.JiaoCaiDetailId);
                    }
                    catch
                    {

                    }
                });
            }

        }


        public void UpdateJiaocaiDetailStatus(int jiaocaiDetailId)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.JiaocaiDetail.Any(t => t.JiaoCaiDetailId == jiaocaiDetailId))
                {
                    var entity = db.JiaocaiDetail.FirstOrDefault(t => t.JiaoCaiDetailId == jiaocaiDetailId);
                    entity.Status = true;
                    db.SaveChanges();
                }
            }
        }

        public bool ExistGrabPageSource(int areaId, int jiaocaiDetailId, int gradeId)
        {
            using (var db = new XKWEntities2())
            {
                return db.QuestionPageSource.Any(
                    t => t.AreaId == areaId && t.JiaocaiDetailId == jiaocaiDetailId && t.GradeId == gradeId);
            }
        }

        public void AddGrabPageSource(int areaId, int jiaocaiDetailId, int gradeId, int total)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.QuestionPageSource.Any(
                        t => t.AreaId == areaId && t.JiaocaiDetailId == jiaocaiDetailId && t.GradeId == gradeId))
                    return;
                var entity = new QuestionPageSource();
                entity.AreaId = areaId;
                entity.JiaocaiDetailId = jiaocaiDetailId;
                entity.GradeId = gradeId;
                entity.Total = total;
                db.QuestionPageSource.Add(entity);
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
                return db.V_JiaocaiDetailYSY.Any(t => t.IsLevel1 && t.Status == false);
            }
        }
        public List<V_JiaocaiDetailYSY> GetRandom10CrawlerJC()
        {
            using (var db = new XKWEntities2())
            {

                return
                    db.Database.SqlQuery<V_JiaocaiDetailYSY>(
                        @"
select top 10 *, NewID() as random from [V_JiaocaiDetailYSY] where status=0 order by random").ToList();
                return db.V_JiaocaiDetailYSY.Where(t => t.IsLevel1 && t.Status == false)/*.Where(t => t.SubjectName.Contains("语文") || t.SubjectName.Contains("数学") || t.SubjectName.Contains("英语"))*/.ToList();
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
