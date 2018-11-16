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
    public static class GrabAnswers
    {

        public static List<string> GetProxyListFromCache()
        {
            var fullName = MemoryCacheHelper.GetCacheItem<List<string>>("proxyList",
               delegate () { return GetProxyListFromBy(); },
               new TimeSpan(0, 10, 0));//30分钟过期
            return fullName;

        }

        public static List<string> UnusefulProxy = new List<string>();

        public static List<string> GetProxyListFromBy()
        {
            var res = HttpClientHolder.GetRequest("http://www.flnsu.com/ip.php?key=9172343651&tqsl=500");

            return res.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

        }


        public static List<V_ALL_CategoryUrlList> GetCatePageList()
        {
            using (var db = new CrawlerEntities())
            {
                return db.V_ALL_CategoryUrlList.Where(t => t.Status == false).Take(100).ToList();
                //                return db.Database.SqlQuery<V_ALL_CategoryUrlList>(@"select top 1000 * from [dbo].[V_ALL_CategoryUrlList] where id not in (select distinct CategoryUrlListId from [dbo].[Question])
                //").ToList();
                //return db.V_ALL_CategoryUrlList.Where(t => t.Id == 19154).ToList();
            }
        }


        public static List<Question> GetTopQuestion()
        {

            using (var db = new CrawlerEntities())
            {
                var data = db.Database.SqlQuery<Question>(@"
declare @Rowid table(rowid int);
BEGIN
set rowcount 300; --一次读取的行数
--先将要读取的记录状态更新
update Question set[IsGrabAns] = 1 output deleted.ID into @Rowid Where[IsGrabAns] = 0 and IsRemoteDelete = 0 and question_id=1604638;


                --读取刚更新状态的记录
select* from Question where ID in (select Rowid from @Rowid);
                END").ToList();
                //return db.Question.Where(t => t.IsGrabAns == false && t.IsRemoteDelete == false).Take(200).ToList();
                return data;
            }
        }

        public static void StartSync()
        {
            while (GetCatePageList().Count > 0)
            {
                var question = GetCatePageList();

                List<Action> ll = new List<Action>();
                foreach (var q in question)
                {
                    Console.WriteLine($"{q.CategoryName}    {q.PageNum}");
                    ProcessPageQuestion(q);
                    DataService.UpdateQuestionStatus(q.Id);
                }

                Parallel.Invoke(ll.ToArray());
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
            catch (InvalidOperationException oi)
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
                }

            }
            catch (Exception ex)
            {

            }
        }


        public static void CrawlerAnswer(Question item)
        {

            //var answer = new QuestionParser().ParseAnswer(item.QuestionId.ToString());
            var answerHttpClient = CrawlerSingleQuestion(item.Question_Id.ToString());
            if (answerHttpClient == null || string.IsNullOrWhiteSpace(answerHttpClient.ToString()))
            {
                return;
            }


            DataService.UpdateQuestionFromCrawler(item.Question_Id, answerHttpClient[0]["questions"][0]["title"].ToString(),
                 answerHttpClient[0]["questions"][0]["answer"].ToString(),
                 answerHttpClient[0]["questions"][0]["knowledge"].ToString(),
                 answerHttpClient[0]["questions"][0]["question_from"].NullToString(),
                 answerHttpClient[0]["questions"][0]["question_source"].NullToString(), answerHttpClient[0]["questions"][0]["score"]["score"].NullToString(),
                  string.Format("https://www.zujuan.com/question/detail-{0}.shtml", item.Question_Id),
                  answerHttpClient.ToString()
                 );
        }

        public static JArray CrawlerSingleQuestion(string questionId)
        {
            try
            {
#if DEBUG
                var a = HttpClientHolder.GetRequest($"https://www.zujuan.com/question/detail-{questionId}.shtml");
#else
                var a = HttpClientHolder.Proxy_GetRequestAbyyun($"https://www.zujuan.com/question/detail-{questionId}.shtml");
#endif

                //
                if (a.IndexOf("试题已经被删除") >= 0)
                {
                    Console.WriteLine($"{questionId} has delete!!!");
                    using (var db = new CrawlerEntities())
                    {
                        var id = questionId.NullToInt();
                        var entity = db.Question.FirstOrDefault(t => t.Question_Id == id);
                        if (entity != null)
                        {
                            entity.IsGrabAns = true;
                            entity.IsRemoteDelete = true;
                            db.SaveChanges();
                        }
                    }
                    return null;
                }
                if (a.IndexOf("限制访问试题") >= 0)
                {
                    using (var db = new CrawlerEntities())
                    {
                        var id = questionId.NullToInt();
                        var entity = db.Question.FirstOrDefault(t => t.Question_Id == id);
                        if (entity != null)
                        {
                            entity.IsGrabAns = false;
                            db.SaveChanges();
                        }
                    }
                    return null;
                }
                Console.WriteLine($"start crawler https://www.zujuan.com/question/detail-{questionId}.shtml ");
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
                using (var db = new CrawlerEntities())
                {
                    var id = questionId.NullToInt();
                    var entity = db.Question.FirstOrDefault(t => t.Question_Id == id);
                    if (entity != null)
                    {
                        entity.IsRemoteDelete = false;
                        entity.IsGrabAns = false;
                        db.SaveChanges();
                    }
                }
            }
            catch (JsonReaderException je)
            {
                using (var db = new CrawlerEntities())
                {
                    var id = questionId.NullToInt();
                    var entity = db.Question.FirstOrDefault(t => t.Question_Id == id);
                    if (entity != null)
                    {
                        entity.IsRemoteDelete = false;
                        entity.IsGrabAns = false;
                        db.SaveChanges();
                    }
                }
            }
            catch (WebException we)
            {
                using (var db = new CrawlerEntities())
                {
                    var id = questionId.NullToInt();
                    var entity = db.Question.FirstOrDefault(t => t.Question_Id == id);
                    if (entity != null)
                    {
                        entity.IsRemoteDelete = false;
                        entity.IsGrabAns = false;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                using (var db = new CrawlerEntities())
                {
                    var id = questionId.NullToInt();
                    var entity = db.Question.FirstOrDefault(t => t.Question_Id == id);
                    if (entity != null)
                    {
                        entity.IsRemoteDelete = false;
                        entity.IsGrabAns = false;
                        db.SaveChanges();
                    }
                }
            }
            //UpdateQuestionGrabStatus(questionId.NullToInt());
            return null;
        }


        public static void UpdateUnGrabQuestionStatus()
        {
            using (var db = new CrawlerEntities())
            {
                db.Database.ExecuteSqlCommand(
                    @"update  question set IsGrabAns=0 where AnswerJson is  null and IsRemoteDelete=0 and IsGrabAns=1");
            }
        }
    }
}