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
using System.Web;
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
                Parallel.ForEach(listJiaocai, (source) =>
                  {
                      try
                      {
                          var total = source.Total;
                          var pageCount = total / 10 + 1;

                          Parallel.For(1, pageCount + 1, (i) =>
                                   {
                                       if (ExistQuestionJiaocaiSourceResult(source.Id, i))
                                           return;

                                       var url =
                                              $"http://zujuan.xkw.com/{source.Prefix}/zj{source.JiaocaiId}/a{source.AreaId}p{i}/";


                                       Console.WriteLine(url);

                                       //CookieCollection cc = new CookieCollection();
                                       //cc.Add(new System.Net.Cookie("bankId", source.SubjectId.ToString(), "/", "zujuan.xkw.com"));
                                       //cc.Add(new System.Net.Cookie("bankname", source.SubjectName.ToString(), "/", "zujuan.xkw.com"));
                                       var html = HttpWebResponseProxyFLNSU.ExecuteCreateGetHttpResponseProxy(url, 12000, $" isshowAnswer=false; bankname={source.Prefix};  bankId={source.SubjectId.ToString()};");
                                       //var html = HttpWebResponseUtility.ExecuteCreateGetHttpResponseProxy(url, 3000, HttpUtility.UrlEncode( $"bankname={source.Prefix.ToString()};bankId={source.SubjectId.ToString()};categoryId=58550;categoryClick=58550;UM_distinctid=166e9518cbe298-01a36d572eaa51-43480420-144000-166e9518cbf41c; isshowAnswer=false;allbankCount=0%2c0; Hm_lpvt_68fb48a14b4fce9d823df8a437386f93=1541947025; _cnzz_CV1274198201=%E6%98%AF%E5%90%A6%E7%99%BB%E5%BD%95%7C%E6%9C%AA%E7%99%BB%E5%BD%95%7C1541947026394;ASP.NET_SessionId=ztqe22oqdjmbv1fsrru1il3i; "));
                                       //var html = HttpWebResponseUtility.ExecuteCreateGetHttpResponseProxy(url, 3000, "UM_distinctid=166e9518cbe298-01a36d572eaa51-43480420-144000-166e9518cbf41c; isshowAnswer=false; bankname=czsx; pro_bank=0%2412; bankId=2; categoryClick=58550; categoryId=58550; CNZZDATA1261546733=2039576993-1541507459-https%253A%252F%252Fwww.baidu.com%252F%7C1541945470; pts=%2fczsx%2fzj58550%2fpts1a610000%2f; ds=%2fczsx%2fzj58550%2fds1a610000%2f; CNZZDATA1274198201=1092295544-1541511793-https%253A%252F%252Fwww.baidu.com%252F%7C1541946465; cn_5816665539db22708e01_dplus=%7B%22distinct_id%22%3A%20%22166e9518cbe298-01a36d572eaa51-43480420-144000-166e9518cbf41c%22%2C%22sp%22%3A%20%7B%22%24_sessionid%22%3A%200%2C%22%24_sessionTime%22%3A%201541946858%2C%22%24dp%22%3A%200%2C%22%24_sessionPVTime%22%3A%201541946858%2C%22%24recent_outside_referrer%22%3A%20%22%24direct%22%7D%2C%22initial_view_time%22%3A%20%221541513972%22%2C%22initial_referrer%22%3A%20%22http%3A%2F%2Fzujuan.xkw.com%2Fgzsx%2Fzsd27929%2Fpt3%2F%22%2C%22initial_referrer_domain%22%3A%20%22zujuan.xkw.com%22%7D; ASPSESSIONIDAQQRSDDD=ICJPMAGDNJMDDHFODCEOJIJD; ASP.NET_SessionId=ztqe22oqdjmbv1fsrru1il3i; Hm_lvt_68fb48a14b4fce9d823df8a437386f93=1541678249,1541797743,1541907309,1541946958; allbankCount=0%2c0; Hm_lpvt_68fb48a14b4fce9d823df8a437386f93=1541947025; _cnzz_CV1274198201=%E6%98%AF%E5%90%A6%E7%99%BB%E5%BD%95%7C%E6%9C%AA%E7%99%BB%E5%BD%95%7C1541947026394");
                                       if (html.IndexOf("queslistbox") < 0)
                                       {
                                           throw new Exception("invalid html result");
                                       }
                                       if (html.IndexOf("拒绝访问") >= 0)
                                       {
                                           ProxyManager.HasExpire = true;
                                           Console.WriteLine("invalid proxy");
                                           throw new Exception("invalid proxy");
                                       }
                                       if (html.IndexOf("涉嫌恶意操作") >= 0)
                                       {
                                           ProxyManager.HasExpire = true;
                                           Console.WriteLine("invalid proxy");
                                           throw new Exception("invalid proxy");
                                       }
                                       if (html.IndexOf("questioncount") < 0)
                                       {
                                           throw new Exception("invalid html result");
                                       }

                                       var questionHtml = NSoupClient.Parse(html).GetElementById("queslistbox").Html();
                                       var questions = NSoupClient.Parse(html).GetElementById("queslistbox").GetElementsByClass("quesbox");
                                       if (i < pageCount - 1 && questions.Count < 10)
                                       {
                                           throw new Exception();
                                       }
                                       var doc = NSoupClient.Parse(html);
                                       var elements = doc.GetElementsByTag("img");
                                       new ParseQuestionXkw().SaveImage(elements);
                                       //////
                                       var listQuestion = new ParseQuestionXkw().AddQuestion(html, questionHtml, source.JiaocaiId, source.SubjectId.ToString(), url, source.AreaId, source.Id, source.Total, i);
                                       //var listQuestion = new ParseQuestionXkw().AddQuestion(html, source.JiaocaiId, source.SubjectId.ToString(), url);
                                       ////AddQuestion(questionHtml, source.Id, url);
                                       //AddQuestionJiaocaiSourceResult(source.AreaId, source.JiaocaiId, source.Id, questionHtml, source.Total, i, url, listQuestion);
                                   });

                          UpdateQuestionJiaocaiSourceResultStatus(source.Id);
                      }
                      catch
                      {

                      }
                      finally { }
                  });


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

        public void AddQuestionJiaocaiSourceResult(int areaId, int jiaocaiId, int sourceId, string html, int total, int pageNum, string crawlerUrl, List<QuestionXkw> entities)
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

                foreach (var item in entities)
                {
                    QuestionJiaocaiSourceResultQuestion question = new QuestionJiaocaiSourceResultQuestion();
                    question.QuestionId = item.QuestionId;
                    question.QuestionJiaocaiSourceResultId = entity.Id;
                    question.CreateTime = DateTime.Now;
                    db.QuestionJiaocaiSourceResultQuestion.Add(question);
                    db.SaveChanges();
                }
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
select top 2 *, NewID() as random from [V_QuestionJiaoCaiSource] where status=0 and total>0 and total<=10000 order by random").ToList();
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
