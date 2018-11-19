using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Common;
using Grabzujuan;
using NSoup;

namespace CrawlerXKW
{
    class Program
    {
        static void Main(string[] args)
        {
            //new CrawlerSubject().Crawler();
            //new CrawlerJiaocai().Crawler();
            //new CrawlerJiaocaiDetail().Crawler();
            //new CrawlerQuestionJiaoCaiPageList().Crawler();
            new Task(() =>
            {
                new CrawlerQuestion().Crawler();
            }).Start();
            while (( DateTime.Now - HttpWebResponseProxyFLNSU.DtLastSuccessTime).TotalSeconds <= 40)
            {
                System.Threading.Thread.Sleep(3000);
            }
            System.Windows.Forms.Application.Restart();
            //var res = HttpClientHolder.GetRequest("http://zujuan.xkw.com/gzyw/zj75639/");

            //new ParseQuestionXkw().TestDown();
        }

        public static void StartImage()
        {
            var list = GetList();

            foreach (var item in list)
            {
                try
                {
                    var doc = NSoupClient.Parse(item);
                    var elements = doc.GetElementsByTag("img");
                    new ParseQuestionXkw().SaveImage(elements);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static List<string> GetList()
        {
            using (var db = new XKWEntities2())
            {
                return db.QuestionXkw.Select(t => t.OriginHtml).ToList();
            }

        }
    }
}
