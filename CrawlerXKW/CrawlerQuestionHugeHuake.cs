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
    public class CrawlerQuestionHugeHuake
    {
        public void Crawler()
        {
            while (ExistCrawlerJcSource())
            {
                var listJiaocai = GetRandom10CrawlerJcSource();
                Parallel.ForEach(listJiaocai, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, (source) =>
                     {
                         try
                         {
                             var total = source.Total;
                             var pageCount = total / 10 + 1;

                             Parallel.For(1, pageCount + 1, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, (i) =>
                                      {
                                          if (ExistQuestionJiaocaiSourceResult(source.Id, i))
                                              return;

                                          var url =
                                                 $"http://zujuan.xkw.com/{source.Prefix}/zj{source.JiaocaiDetailId}/a{source.AreaId}p{i}/";


                                          Console.WriteLine(url);

                                          //CookieCollection cc = new CookieCollection();
                                          //cc.Add(new System.Net.Cookie("bankId", source.SubjectId.ToString(), "/", "zujuan.xkw.com"));
                                          //cc.Add(new System.Net.Cookie("bankname", source.SubjectName.ToString(), "/", "zujuan.xkw.com"));
                                          var html = HttpWebResponseProxyHuake.ExecuteCreateGetHttpResponseProxy(url, 18000, $" isshowAnswer=false; bankname={source.Prefix};  bankId={source.SubjectId.ToString()};");
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
                                          var doc = NSoupClient.Parse(html);
                                          var questionHtml = doc.GetElementById("queslistbox").Html();
                                          var questions = doc.GetElementById("queslistbox").GetElementsByClass("quesbox");
                                          if (i < pageCount - 1 && questions.Count < 10)
                                          {
                                              var totalCount = doc.GetElementById("questioncount").Text().NullToInt();
                                              if (source.Total != totalCount)
                                              {
                                                  UpdateQuestionJiaocaiDetailSourceTotalCount(source.Id, totalCount);
                                              }
                                              throw new Exception();
                                          }
                                        
                                          var elements = doc.GetElementsByTag("img");
                                          var dicImageStatus = new ParseQuestionXkw().SaveImage(elements, url);
                                          //////
                                          var listQuestion = new ParseQuestionXkw().AddQuestion2(html, questionHtml, source.JiaocaiId, source.SubjectId.ToString(), url, source.AreaId, source.Id, source.Total, i, source.Id, dicImageStatus);
                                          //AddQuestion(questionHtml, source.Id, url);
                                          //AddQuestionJiaocaiSourceResult(source.AreaId, source.JiaocaiId, source.Id, questionHtml, source.Total, i, url, listQuestion);
                                      });

                             UpdateQuestionJiaocaiDetailSourceResultStatus(source.Id);
                         }
                         catch
                         {

                         }
                         finally { }
                     });


            }

        }

        public void UpdateQuestionJiaocaiDetailSourceTotalCount(int id, int total)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.QuestionJiaoCaiDetailSource.Any(t => t.Id == id))
                {
                    var entity = db.QuestionJiaoCaiDetailSource.FirstOrDefault(t => t.Id == id);
                    entity.Total = total;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateQuestionJiaocaiDetailSourceResultStatus(int id)
        {
            using (var db = new XKWEntities2())
            {
                if (
                    db.QuestionJiaoCaiDetailSource.Any(t => t.Id == id))
                {
                    var entity = db.QuestionJiaoCaiDetailSource.FirstOrDefault(t => t.Id == id);
                    entity.Status = true;
                    db.SaveChanges();
                }
            }
        }

        public bool ExistQuestionJiaocaiSourceResult(int QuestionJiaoCaiDetailSourceId, int pageNum)
        {
            using (var db = new XKWEntities2())
            {
                return db.QuestionJiaocaiSourceDetailResult.Any(
                    t => t.QuestionJiaoCaiDetailSourceId == QuestionJiaoCaiDetailSourceId && t.PageNum == pageNum);
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
                var entity = new QuestionJiaocaiSourceDetailResult();
                entity.AreaId = areaId;
                entity.JiaocaiId = jiaocaiId;
                entity.Html = html;
                entity.Total = total;
                entity.PageNum = pageNum;
                entity.QuestionJiaoCaiDetailSourceId = sourceId;
                entity.CrawlerUrl = crawlerUrl;
                db.QuestionJiaocaiSourceDetailResult.Add(entity);
                db.SaveChanges();

            }
        }
        public bool ExistCrawlerJcSource()
        {
            using (var db = new XKWEntities2())
            {
                return db.V_QuestionJiaoCaiDetailSource.Any(t => t.Status == false && t.Total > 0);
            }
        }
        public List<V_QuestionJiaoCaiDetailSource> GetRandom10CrawlerJcSource()
        {
            using (var db = new XKWEntities2())
            {

                return
                    db.Database.SqlQuery<V_QuestionJiaoCaiDetailSource>(
                        @"
select top 2 *, NewID() as random from [V_QuestionJiaoCaiDetailSource] where status=0 and total>0  order by random").ToList();
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
    }
}
