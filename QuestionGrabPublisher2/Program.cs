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
            StartCrawler();

        }

        public static void StartCrawler()
        {
            try
            {
                var proxys = GrabAnswer.GetProxyListFromBy();
                var q = GetTopQuestion1();
                List<Action> ll = new List<Action>();
                GrabAnswer.CrawlerAnswer(q, proxys);
            }
            catch (Exception)
            {


            }
            finally
            {
                System.Windows.Forms.Application.Restart();
            }
            //while (GrabAnswer.GetTopQuestion().Count > 0)
            //{
            //    var listQuestion = GrabAnswer.GetTopQuestion();
            //    CacheManager.RPush("QUESTION", listQuestion);
            //    foreach (var q in listQuestion)
            //    {
            //        DataService.UpdateQuestionQueueStatus(q.Question_Id);
            //    }
            //}


            //List<Question> listQuestion;
            //do
            //{
            //    listQuestion = GetTopQuestion();
            //    var proxys = GrabAnswer.GetProxyListFromBy();
            //    Parallel.ForEach(listQuestion, (q) =>
            //    {
            //        GrabAnswer.CrawlerAnswer(q, proxys);
            //    });
            //} while (listQuestion.Count > 0);
            //while (GetTopQuestion().Count > 0)
            //{
            //    var listQuestion = GetTopQuestion();
            //    Parallel.ForEach(listQuestion, (q) =>
            //    {
            //        GrabAnswer.CrawlerAnswer(q);
            //    });
            //}

        }


        public static List<QuestionAll> GetTopQuestion()
        {
            using (var db = new CrawlerEntities())
            {
                return db.Database.SqlQuery<QuestionAll>(@"
select  top 100 * into #temp from  QuestionAll where IsGrabAns =0 and IsRemoteDelete=0
update QuestionAll set IsGrabAns=1 where id in (select id from #temp)

select * from #temp
").ToList();
                //return db.Question.Where(t => t.IsGrabAns == false && t.IsRemoteDelete == false).Take(500).ToList();
            }
        }

        public static QuestionAll GetTopQuestion1()
        {
            using (var db = new CrawlerEntities())
            {
                return db.QuestionAll.Where(t => t.QuestionId == 615137).FirstOrDefault();
                //return db.Question.Where(t => t.IsGrabAns == false && t.IsRemoteDelete == false).Take(500).ToList();
            }
        }
    }



}
