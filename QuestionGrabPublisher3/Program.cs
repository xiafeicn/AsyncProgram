using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Grabzujuan;
using Grabzujuan.Common;
using YGJJ.Core.Cache;

namespace QuestionGrabPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var proxys =GetProxyListFromBy();
                var listQuestion = GrabAnswers.GetTopQuestion();
                Parallel.ForEach(listQuestion, (q) =>
                {
                    GrabAnswers.CrawlerAnswer(q, proxys);
                });
            }
            catch (Exception)
            {
            }
            //while (GetTopQuestion().Count > 0)
            //{

            //}
            finally
            {
                System.Windows.Forms.Application.Restart();
            }
        }
        public static List<string> GetProxyListFromBy()
        {
            var res = HttpClientHolder.GetRequest("http://ip.yixrp.cn/vip.php?key=61089451273&sl=500");

            return res.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

        }
        //        public static void StartCrawler()
        //        {
        //            try
        //            {
        //                var proxys = GrabAnswer.GetProxyListFromBy();
        //                var question = GetTopQuestion();

        //                List<Action> ll = new List<Action>();
        //                foreach (var q in question)
        //                {
        //                    Action a = new Action(() =>
        //                    {
        //                        GrabAnswer.CrawlerAnswer(q, proxys);
        //                    });
        //                    ll.Add(a);
        //                }

        //                Parallel.Invoke(ll.ToArray());
        //            }
        //            catch (Exception)
        //            {


        //            }
        //            finally
        //            {
        //                System.Windows.Forms.Application.Restart();
        //            }
        //            //while (GrabAnswer.GetTopQuestion().Count > 0)
        //            //{
        //            //    var listQuestion = GrabAnswer.GetTopQuestion();
        //            //    CacheManager.RPush("QUESTION", listQuestion);
        //            //    foreach (var q in listQuestion)
        //            //    {
        //            //        DataService.UpdateQuestionQueueStatus(q.Question_Id);
        //            //    }
        //            //}


        //            //List<Question> listQuestion;
        //            //do
        //            //{
        //            //    listQuestion = GetTopQuestion();
        //            //    var proxys = GrabAnswer.GetProxyListFromBy();
        //            //    Parallel.ForEach(listQuestion, (q) =>
        //            //    {
        //            //        GrabAnswer.CrawlerAnswer(q, proxys);
        //            //    });
        //            //} while (listQuestion.Count > 0);
        //            //while (GetTopQuestion().Count > 0)
        //            //{
        //            //    var listQuestion = GetTopQuestion();
        //            //    Parallel.ForEach(listQuestion, (q) =>
        //            //    {
        //            //        GrabAnswer.CrawlerAnswer(q);
        //            //    });
        //            //}

        //        }


        //        public static List<QuestionAll> GetTopQuestion()
        //        {
        //            using (var db = new CrawlerEntities())
        //            {
        //                return db.Database.SqlQuery<QuestionAll>(@"
        //update [Question] set [IsGrabAns]=1 where  AnswerJson not like '' and [IsGrabAns]=0
        //select  top 100 * into #temp from  QuestionAll where IsGrabAns =0 and IsRemoteDelete=0
        //update QuestionAll set IsGrabAns=1 where id in (select id from #temp)

        //select * from #temp
        //").ToList();
        //                //return db.Question.Where(t => t.IsGrabAns == false && t.IsRemoteDelete == false).Take(500).ToList();
        //            }
        //        }
    }
}
