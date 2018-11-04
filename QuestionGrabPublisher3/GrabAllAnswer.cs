using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using Grabzujuan.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YGJJ.Core.Cache;

namespace Grabzujuan
{
    public static class GrabAllAnswer
    {
        public static string key = "crawler_all_question";

        public static List<string> UnusefulProxy = new List<string>();



        public static void StartWithIndex()
        {
            int qid = CacheManager.IncrementValue(key).NullToInt();
            if (qid > 10000000)
            {
                System.Windows.Forms.Application.Exit();
                return;
            }
            if (!IsCrawlered(qid))
            {
                var result = HttpClientHolder.Execute($"https://www.zujuan.com/question/detail-{qid}.shtml");
                if (result.IndexOf("试题已经被删除") >= 0)
                {
                    AddDeleteQuestion(qid);
                }
                else
                {
                    CrawlerAnswer(qid);
                }
            }
        }

        public static bool IsCrawlered(long questionId)
        {
            using (var db = new CrawlerEntities())
            {
                return db.QuestionAll.Any(t => t.QuestionId == questionId);
            }
        }
        public static void AddDeleteQuestion(int questionId)
        {
            using (var db = new CrawlerEntities())
            {
                var entity = new QuestionAll();
                entity.QuestionId = questionId;
                entity.IsDelete = true;
                db.QuestionAll.Add(entity);
                db.SaveChanges();
            }
        }
        public static void CrawlerAnswer(int qid)
        {
            var proxys = GrabAnswer.GetProxyListFromCache();
            //var answer = new QuestionParser().ParseAnswer(item.QuestionId.ToString());
            var answerHttpClient = CrawlerSingleQuestion(qid, proxys);
            if (answerHttpClient == null || string.IsNullOrWhiteSpace(answerHttpClient.ToString()))
            {
                return;
            }
            var xd = answerHttpClient[0]["questions"][0]["xd"].ToString();
            var chid = answerHttpClient[0]["questions"][0]["chid"].ToString();

            var apiUrl = $"https://www.zujuan.com/question/list?question_id={qid}&xd={xd}&chid={chid}";
            var apiJson = HttpClientHolder.Execute(apiUrl);
            using (var db = new CrawlerEntities())
            {
                var entity = new QuestionAll();
                entity.QuestionId = qid;
                entity.IsDelete = false;
                entity.AnswerJson = answerHttpClient.ToString();
                entity.ApiJson = apiJson;
                entity.CrawlerUrl = $"https://www.zujuan.com/question/detail-{qid}.shtml";
                entity.CrawlerApiUrl = apiUrl;
                entity.child = chid.NullToInt();
                entity.xd = xd.NullToInt();

                db.QuestionAll.Add(entity);
                db.SaveChanges();
            }

        }

        public static JArray CrawlerSingleQuestion(int questionId, List<string> proxys)
        {
            var proxy = proxys[new Random().Next(0, proxys.Count - 1)];
            try
            {
                var a = HttpClientHolder.Proxy_GetRequest($"https://www.zujuan.com/question/detail-{questionId}.shtml",
                    proxy);
                if (a.IndexOf("试题已经被删除") >= 0)
                {
                   // AddDeleteQuestion(questionId);
                }
                if (a.IndexOf("限制访问试题") >= 0)
                {
                    throw new Exception("test");
                }
                Console.WriteLine($"start crawler https://www.zujuan.com/question/detail-{questionId}.shtml  {proxy}");
                //例如我想提取记录中的NAME值
                string value = GrabAnswer.GetValue(a, "var MockDataTestPaper =", "OT2.renderQList").TrimEnd(new char[] { ';' });
                value = value.Trim().TrimEnd(new char[] { ';' }).Trim();
                //; UpdateProxGrabyime(proxy.Id);

                //更新代理时间

                return JArray.Parse(value);
            }
            catch (IOException io)
            {
                if (!UnusefulProxy.Any(t => t.Equals(proxy)))
                    UnusefulProxy.Add(proxy);
            }
            catch (JsonReaderException je)
            {
                //AddDeleteQuestion(questionId);
            }
            catch (WebException we)
            {
                //if (we.Status == WebExceptionStatus.ConnectFailure || we.Status == WebExceptionStatus.ProtocolError)
                //{
                if (!UnusefulProxy.Any(t => t.Equals(proxy)))
                    UnusefulProxy.Add(proxy);
                //SetProxyDisable(proxy.Id);

                //}
            }
            return null;
        }
    }
}
