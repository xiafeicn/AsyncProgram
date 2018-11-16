using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Grabzujuan;
using Grabzujuan.Common;

namespace QuestionGrabPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                if (AllCount() > 1000)
                {
                    Console.WriteLine("更新！！！");
                    Update();
                }

                System.Threading.Thread.Sleep(1 * 60 * 1000);
            }
            Console.ReadKey();
        }


        static int AllCount()
        {
            using (var db = new CrawlerEntities())
            {
                return
                    db.Database.SqlQuery<int>(
                        "select isnull(count(1),0) from question where AnswerJson is  null and  IsGrabAns=1 and IsRemoteDelete=0").FirstOrDefault();
            }
        }

        static void Update()
        {
            using (var db = new CrawlerEntities())
            {

                db.Database.ExecuteSqlCommand(
                    "update  question set IsGrabAns=0 where AnswerJson is  null and  IsGrabAns=1 and IsRemoteDelete=0");
            }
        }
    }
}
