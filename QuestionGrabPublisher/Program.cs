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

                var listQuestion = GrabAnswers.GetTopQuestion();

                Action action1 = () =>
                {
                    var proxys = GrabAnswers.GetProxyListFromBy();
                    Parallel.ForEach(listQuestion.Skip(0).Take(200), (q) =>
                    {
                        GrabAnswers.CrawlerAnswer(q, proxys);
                    });
                };
                Action action2 = () =>
                {
                    var proxys = GrabAnswers.GetProxyListFromBy();
                    Parallel.ForEach(listQuestion.Skip(0).Take(200), (q) =>
                    {
                        GrabAnswers.CrawlerAnswer(q, proxys);
                    });
                };
                Action action3 = () =>
                {
                    var proxys = GrabAnswers.GetProxyListFromBy();
                    Parallel.ForEach(listQuestion.Skip(400).Take(200), (q) =>
                    {
                        GrabAnswers.CrawlerAnswer(q, proxys);
                    });
                };
                Action action4 = () =>
                {
                    var proxys = GrabAnswers.GetProxyListFromBy();
                    Parallel.ForEach(listQuestion.Skip(600).Take(200), (q) =>
                    {
                        GrabAnswers.CrawlerAnswer(q, proxys);
                    });
                };
                Action action5 = () =>
                {
                    var proxys = GrabAnswers.GetProxyListFromBy();
                    Parallel.ForEach(listQuestion.Skip(800).Take(200), (q) =>
                    {
                        GrabAnswers.CrawlerAnswer(q, proxys);
                    });
                };
                Parallel.Invoke(action1, action2, action3, action4, action5);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            //while (GetTopQuestion().Count > 0)
            //{

            //}
            finally
            {
                System.Windows.Forms.Application.Restart();
            }
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
