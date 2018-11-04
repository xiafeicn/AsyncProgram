using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Grabzujuan;
using Newtonsoft.Json;
using YGJJ.Core.Cache;

namespace QuestionGrabSubscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = JsonConvert.SerializeObject(GetQuestionList());
            File.WriteAllText("D:\\1.txt", str);
            //List<Question> item;
            //do
            //{
            //    item = CacheManager.DequeueItemFromList<List<Question>>("QUESTION");
            //    if (item != null)
            //    {
            //        try
            //        {
            //            Parallel.ForEach(item, (q) =>
            //            {
            //                GrabAnswer.CrawlerAnswer(q);
            //            });
            //        }
            //        catch (Exception e)
            //        {

            //        }
            //    }
            //} while (item != null);
        }

        public static List<V_ALL_CrawlerQuestion> GetQuestionList()
        {
            using (var db = new CrawlerEntities())
            {
                return db.V_ALL_CrawlerQuestion.Take(1000).ToList();
            }
        }
    }
}
