using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
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
    public class CrawlerQuestion
    {
        public void Crawler()
        {
            while (ExistCrawlerJcSource())
            {
                var listJiaocai = GetRandom10CrawlerJcSource();
                Parallel.ForEach(listJiaocai.Take(1), (source) =>
                {
                    try
                    {
                        var total = source.Total;
                        var pageCount = total / 10 + 1;

                        Parallel.For(1, 2, (i) =>
                        {
                            if (ExistQuestionJiaocaiSourceResult(source.Id, i))
                                return;

                            var url =
                                   $"http://zujuan.xkw.com/{source.Prefix}/zj{source.JiaocaiId}/a{source.AreaId}p{i}/";


                            Console.WriteLine(url);

                            CookieCollection cc = new CookieCollection();
                            cc.Add(new System.Net.Cookie("bankId", source.SubjectId.ToString(), "/", "zujuan.xkw.com"));
                            cc.Add(new System.Net.Cookie("bankname", source.SubjectName.ToString(), "/", "zujuan.xkw.com"));
                            var html = HttpWebResponseUtility.ExecuteCreateGetHttpResponseProxy(url, 3000, cc);
                            if (html.IndexOf("queslistbox") < 0)
                            {
                                throw new Exception("invalid html result");
                            }


                            var questionHtml = NSoupClient.Parse(html).GetElementById("queslistbox").Html();
                            AddQuestion(questionHtml, source.Id);
                            AddQuestionJiaocaiSourceResult(source.AreaId, source.JiaocaiId, source.Id, questionHtml, source.Total, i, url);
                        });

                        UpdateQuestionJiaocaiSourceResultStatus(source.Id);
                    }
                    catch
                    {

                    }
                });
            }

        }

        public void AddQuestion(string html, int sourceId)
        {
            var elements = NSoupClient.Parse(html).GetElementsByClass("quesbox");
            foreach (var element in elements)
            {
                try
                {


                    using (var db = new XKWEntities2())
                    {

                        QuestionXkw entity = new QuestionXkw();
                        entity.OriginHtml = element.Html();
                        var detail = element.Select("div.join-sj>a")[0];
                        entity.QuestionId = detail.Attr("quesid").NullToInt();
                        entity.@class = detail.Attr("class").NullToString();
                        entity.guid = detail.Attr("guid").NullToString();
                        entity.childnum = detail.Attr("childnum").NullToInt();
                        entity.questitle = detail.Attr("questitle").NullToString();
                        entity.categories = detail.Attr("categories").NullToString();
                        entity.qyid = detail.Attr("qyid").NullToInt();
                        entity.qdid = detail.Attr("qdid").NullToInt();
                        entity.qyname = detail.Attr("qyname").NullToString();
                        entity.qdname = detail.Attr("qdname").NullToString();

                        var source = element.Select("div.quesource")[0];

                        entity.source = source.Html();


                        var questiontitle = element.Select("div.question-inner")[0];
                        entity.key = questiontitle.Attr("key").NullToString();
                        entity.question_text = questiontitle.Html();

                        var href = element.Select("a.detail")[0];
                        entity.CrawlerUrl = href.Attr("href").NullToString();


                        var str =
                            entity.CrawlerUrl.Replace("http://zujuan.xkw.com/", "").Replace("https://zujuan.xkw.com/", "");
                        var bankId = str.Substring(0, str.IndexOf("q", StringComparison.OrdinalIgnoreCase)); ;
                        var analysisUrl =
                            $"http://im.zujuan.xkw.com/Parse/{entity.QuestionId}/{bankId}/700/14/28/{entity.key}";
                        var answerUrl = $"http://im.zujuan.xkw.com/Answer/{entity.QuestionId}/{bankId}/700/14/28/{entity.key}";



                        entity.QuestionAnalysis = analysisUrl;
                        entity.QuestionAnswer = answerUrl;
                        entity.QuestionJiaoCaiSource = sourceId;
                        entity.CreateTime = DateTime.Now;
                        if (!db.QuestionXkw.Any(t => t.QuestionId == entity.QuestionId))
                        {
                            db.QuestionXkw.Add(entity);
                            db.SaveChanges();
                        }

                    }
                }
                catch(DbUpdateException exception)
                {
                    var msg = string.Empty;

                    foreach (var validationError in ((DbUpdateException) exception).Data)
                    {
                        var o = validationError;
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var msg = string.Empty;

                    foreach (var validationError in ((DbEntityValidationException)ex).EntityValidationErrors)
                        foreach (var error in validationError.ValidationErrors)
                            msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);

                    var fail = new Exception(msg);
                    throw fail;
                }
            }
        }


        public void UpdateQuestionJiaocaiSourceResultStatus(int id)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.QuestionJiaoCaiSource.Any(t => t.Id == id))
                {
                    var entity = db.QuestionJiaoCaiSource.FirstOrDefault(t => t.Id == id);
                    entity.Status = true;
                    db.SaveChanges();
                }
            }
        }

        public bool ExistQuestionJiaocaiSourceResult(int QuestionJiaoCaiSourceId, int pageNum)
        {
            using (var db = new XKWEntities2())
            {
                return db.QuestionJiaocaiSourceResult.Any(
                    t => t.QuestionJiaoCaiSourceId == QuestionJiaoCaiSourceId && t.PageNum == pageNum);
            }
        }

        public void AddQuestionJiaocaiSourceResult(int areaId, int jiaocaiId, int sourceId, string html, int total, int pageNum, string crawlerUrl)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.QuestionJiaocaiSourceResult.Any(
                        t => t.JiaocaiId == jiaocaiId && t.AreaId == areaId && t.PageNum == pageNum))
                    return;
                var entity = new QuestionJiaocaiSourceResult();
                entity.AreaId = areaId;
                entity.JiaocaiId = jiaocaiId;
                entity.Html = html;
                entity.Total = total;
                entity.PageNum = pageNum;
                entity.QuestionJiaoCaiSourceId = sourceId;
                entity.CrawlerUrl = crawlerUrl;
                db.QuestionJiaocaiSourceResult.Add(entity);
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
        public bool ExistCrawlerJcSource()
        {
            using (var db = new XKWEntities2())
            {
                return db.V_QuestionJiaoCaiSource.Any(t => t.Status == false && t.Total > 0 && t.Total <= 10000);
            }
        }
        public List<V_QuestionJiaoCaiSource> GetRandom10CrawlerJcSource()
        {
            using (var db = new XKWEntities2())
            {

                return
                    db.Database.SqlQuery<V_QuestionJiaoCaiSource>(
                        @"
select top 10 *, NewID() as random from [V_QuestionJiaoCaiSource] where jiaocaiid=58550 and status=0 and total>0 and total<=10000 order by random").ToList();
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
