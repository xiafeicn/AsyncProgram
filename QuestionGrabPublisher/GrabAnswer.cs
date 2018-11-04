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

namespace Grabzujuan
{
    public static class GrabAnswer
    {

        public static List<string> GetProxyListFromCache()
        {
            var fullName = MemoryCacheHelper.GetCacheItem<List<string>>("proxyList",
               delegate () { return GetProxyListFromBy(); },
               new TimeSpan(0, 1, 0));//30分钟过期
            return fullName;

        }

        public static List<string> UnusefulProxy = new List<string>();

        public static List<string> GetProxyListFromBy()
        {
            var res = HttpClientHolder.GetRequest("http://www.flnsu.com/ip.php?key=3655275368&tqsl=50");

            return res.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

        }

        public static Proxy GetProxyList()
        {
            using (var db = new CrawlerEntities())
            {
                var limitTime = DateTime.Now.AddMinutes(-5);
                if (db.Proxy.Any(t => (t.LastGrabTime == null || t.LastGrabTime <= limitTime) && t.Disable == false))
                    return db.Proxy.FirstOrDefault(t => (t.LastGrabTime == null || t.LastGrabTime <= limitTime) && t.Disable == false);

                throw new Exception("there is no userful proxy for grab!");
            }
        }

        public static void UpdateProxGrabyime(int proxyId)
        {
            using (var db = new CrawlerEntities())
            {
                var proxy = db.Proxy.FirstOrDefault(t => t.Id == proxyId);
                proxy.LastGrabTime = DateTime.Now;
                db.SaveChanges();
            }
        }
        public static void SetProxyDisable(int proxyId)
        {
            using (var db = new CrawlerEntities())
            {
                var proxy = db.Proxy.FirstOrDefault(t => t.Id == proxyId);
                proxy.Disable = true;
                db.SaveChanges();
            }
        }
        public static List<V_ALL_CategoryUrlList> GetQuestionList()
        {
            using (var db = new CrawlerEntities())
            {
                return db.V_ALL_CategoryUrlList.Where(t => t.Status == false).Take(500).ToList();
            }
        }


        public static List<Question> GetTopQuestion()
        {
            using (var db = new CrawlerEntities())
            {
                return db.Question.Where(t => t.IsGrabAns == false && t.IsRemoteDelete == false).Take(30).ToList();
            }
        }

        public static void UpdateQuestionRemoteStatus(int questionId)
        {
            using (var db = new CrawlerEntities())
            {
                var entity = db.QuestionAll.FirstOrDefault(t => t.QuestionId == questionId);
                if (entity != null)
                {
                    entity.IsRemoteDelete = true;
                    entity.IsGrabAns = true;
                    db.SaveChanges();
                }
            }
        }
        public static void UpdateQuestionGrabStatus(int questionId)
        {
            using (var db = new CrawlerEntities())
            {
                var entity = db.QuestionAll.FirstOrDefault(t => t.QuestionId == questionId);
                if (entity != null)
                {
                    entity.IsGrabAns = false;
                    db.SaveChanges();
                }
            }
        }
        public static void StartSync()
        {
            while (GetQuestionList().Count > 0)
            {
                var question = GetQuestionList();
                foreach (var q in question)
                {
                    ProcessPageQuestion(q);
                }
            }
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }


        public static void ProcessPageQuestion(V_ALL_CategoryUrlList que)
        {
            ConcurrentBag<Question> listEntity = new ConcurrentBag<Question>();
            //List<Grabzujuan.Data.Question> listEntity = new List<Question>();
            if (string.IsNullOrWhiteSpace(que.ApiJson))
            {
                return;
            }
            var json = JObject.Parse(que.ApiJson);
            try
            {
                //var list = json["data"]
                //    .SelectMany(t => t["questions"])
                //    .Select(t => t["question_id"].ToString());
                //items.AddRange(list);
                var list = json["data"]
                    .SelectMany(t => t["questions"]).ToList();
                foreach (var q in list)
                {
                    Question entity = new Question();
                    entity.Question_Id = q["question_id"].ToString().NullToInt();
                    entity.PaperId = q["paperid"].ToString().NullToInt();
                    entity.exam_type = q["exam_type"].ToString().NullToInt();
                    entity.grade_id = q["grade_id"].ToString().NullToInt();
                    entity.difficult_index = q["difficult_index"].ToString().NullToInt();
                    entity.kid_num = q["kid_num"].ToString().NullToInt();
                    entity.QuestionText = q["question_text"].ToString();
                    entity.question_channel_type = q["question_channel_type" +
                                                     ""].ToString().NullToInt();
                    entity.Child = q["chid"].ToString().NullToInt();
                    entity.Degree = q["xd"].ToString().NullToInt();
                    entity.ApiJson = q.ToString();
                    entity.Child = q["chid"].ToString().NullToInt();
                    entity.Degree = q["xd"].ToString().NullToInt();
                    entity.ApiJson = q.ToString();

                    entity.CategoryUrlListId = que.Id;
                    entity.BookId = que.BookId;
                    entity.CategoryId = que.CategoryId;
                    entity.Child = que.Child;
                    entity.CrawlerUrl = "";
                    entity.Degree = que.Degree;
                    entity.QuestionText = q["question_text"].ToString();
                    entity.CrawlerUrl = string.Format("https://www.zujuan.com/question/detail-{0}.shtml", entity.Question_Id);
                    entity.ApiJson = que.ApiJson;
                    entity.Score = q["score"].ToString();
                    listEntity.Add(entity);
                }

            }
            catch
            {
                var quesArray = json["data"].SelectMany(t => t["questions"]).ToList();
                foreach (var ques in quesArray)
                {
                    foreach (var q in ques)
                    {
                        Question entity = new Question();
                        entity.Question_Id = q["question_id"].ToString().NullToInt();
                        entity.PaperId = q["paperid"].ToString().NullToInt();
                        entity.exam_type = q["exam_type"].ToString().NullToInt();
                        entity.grade_id = q["grade_id"].ToString().NullToInt();
                        entity.difficult_index = q["difficult_index"].ToString().NullToInt();
                        entity.kid_num = q["kid_num"].ToString().NullToInt();
                        entity.QuestionText = q["question_text"].ToString();
                        entity.question_channel_type = q["question_channel_type" +
                                                         ""].ToString().NullToInt();
                        entity.Child = q["chid"].ToString().NullToInt();
                        entity.Degree = q["xd"].ToString().NullToInt();
                        entity.ApiJson = q.ToString();

                        entity.CategoryUrlListId = que.Id;
                        entity.BookId = que.BookId;
                        entity.CategoryId = que.CategoryId;
                        entity.Child = que.Child;
                        entity.CrawlerUrl = "";
                        entity.Degree = que.Degree;
                        entity.QuestionText = q["question_text"].ToString();
                        entity.CrawlerUrl = string.Format("https://www.zujuan.com/question/detail-{0}.shtml", entity.Question_Id);
                        entity.ApiJson = que.ApiJson;
                        entity.Score = q["score"].ToString();


                        listEntity.Add(entity);
                        //var sss = k["question_id"];
                        //items.Add(sss.ToString());
                    }
                }
            }
            try
            {

                if (listEntity.Any())
                {
                    DataService.AddQuestion(listEntity.ToList());
                    DataService.UpdateQuestionStatus(que.Id);
                }

            }
            catch (Exception ex)
            {

            }
        }


        public static void CrawlerAnswer(QuestionAll item, List<string> proxys)
        {
            //var answer = new QuestionParser().ParseAnswer(item.QuestionId.ToString());
            var answerHttpClient = CrawlerSingleQuestion(item.QuestionId.ToString(), proxys);
            if (answerHttpClient == null || string.IsNullOrWhiteSpace(answerHttpClient.ToString()))
            {
                //Console.WriteLine($"anserhttp is null {item.QuestionId}");
                //UpdateQuestionGrabStatus(item.QuestionId);
                return;
            }
            var xd = answerHttpClient[0]["questions"][0]["xd"].ToString();
            var chid = answerHttpClient[0]["questions"][0]["chid"].ToString();
            var apiUrl = $"https://www.zujuan.com/question/list?question_id={item.QuestionId}&xd={xd}&chid={chid}";
            var apiJson = HttpClientHolder.Execute(apiUrl, GetCookieState(chid.NullToInt(), xd.NullToInt()));

            Console.WriteLine($"save sql");
            UpdateQuesion(item.QuestionId, answerHttpClient, apiUrl, apiJson, xd, chid);
        }

        public static string GetCookieState(int chid, int xd)
        {
            using (var db = new CrawlerEntities())
            {
                var entity = db.CookieState.FirstOrDefault(t => t.Chid == chid && t.Xd == xd);
                return entity.Cookie;
            }
        }
        public static void UpdateQuesion(int questionid, JArray array, string apiUrl, string apiJson, string xd, string chid)
        {
            using (var db = new CrawlerEntities())
            {
                var entity = db.QuestionAll.FirstOrDefault(t => t.QuestionId == questionid);
                entity.QuestionId = questionid;
                entity.IsDelete = false;
                entity.AnswerJson = array.ToString();
                entity.ApiJson = apiJson;
                entity.CrawlerUrl = $"https://www.zujuan.com/question/detail-{questionid}.shtml";
                entity.CrawlerApiUrl = apiUrl;
                entity.child = chid.NullToInt();
                entity.xd = xd.NullToInt();
                entity.IsGrabAns = true;
                db.SaveChanges();
            }
        }
        public static JArray CrawlerSingleQuestion(string questionId, List<string> proxys)
        {
            var proxy = proxys[new Random().Next(0, proxys.Count - 1)];
            try
            {
                var a = HttpClientHolder.Proxy_GetRequestAbyyun($"https://www.zujuan.com/question/detail-{questionId}.shtml");
                if (a.IndexOf("试题已经被删除") >= 0)
                {
                    Console.WriteLine($"{questionId} has delete!!!");
                    UpdateQuestionRemoteStatus(questionId.ToInt());
                    return null;
                }
                if (a.IndexOf("限制访问试题") >= 0)
                {
                    using (var db = new CrawlerEntities())
                    {
                        var id = questionId.NullToInt();
                        var entity = db.QuestionAll.FirstOrDefault(t => t.QuestionId == id);
                        if (entity != null)
                        {
                            entity.IsGrabAns = false;
                            db.SaveChanges();
                        }
                    }
                    return null;
                }
                Console.WriteLine($"start crawler https://www.zujuan.com/question/detail-{questionId}.shtml  {proxy}");
                //例如我想提取记录中的NAME值
                string value = GetValue(a, "var MockDataTestPaper =", "OT2.renderQList").TrimEnd(new char[] { ';' });
                value = value.Trim().TrimEnd(new char[] { ';' }).Trim();
                //; UpdateProxGrabyime(proxy.Id);
                Console.WriteLine($"aleady get {questionId} return value");
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
                UpdateQuestionRemoteStatus(questionId.ToInt());
            }
            catch (WebException we)
            {
                using (var db = new CrawlerEntities())
                {
                    var id = questionId.NullToInt();
                    var entity = db.QuestionAll.FirstOrDefault(t => t.QuestionId == id);
                    if (entity != null)
                    {
                        entity.IsGrabAns = false;
                        db.SaveChanges();
                    }
                }
                //if (we.Status == WebExceptionStatus.ConnectFailure || we.Status == WebExceptionStatus.ProtocolError)
                //{
                if (!UnusefulProxy.Any(t => t.Equals(proxy)))
                    UnusefulProxy.Add(proxy);
                //SetProxyDisable(proxy.Id);

                //}
            }
            catch (Exception e)
            {
                using (var db = new CrawlerEntities())
                {
                    var id = questionId.NullToInt();
                    var entity = db.QuestionAll.FirstOrDefault(t => t.QuestionId == id);
                    if (entity != null)
                    {
                        entity.IsGrabAns = false;
                        db.SaveChanges();
                    }
                }
            }
            //UpdateQuestionGrabStatus(questionId.NullToInt());
            return null;
        }
    }
}
