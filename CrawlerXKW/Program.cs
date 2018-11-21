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

            //var res = HttpClientHolder.GetRequest("http://zujuan.xkw.com/gzyw/zj75639/");

            //new ParseQuestionXkw().TestDown();
            //new ExportTest().export();
            //new HtmlTest().TestParseQuestion();

            ///*11111111111111111111111*/
            //Part1();
            //System.Windows.Forms.Application.Restart();

            /*11111111111111111111111*/
            Part2();
            System.Windows.Forms.Application.Restart();
            //new QuestionJiaoCaiDetailSourceCrawler().StartCrawler();
            //new ImageDownloadTest().test();
        }

        public static void Part1()
        {
            new Task(() =>
            {
                new CrawlerQuestionHuake().Crawler();
            }).Start();
            while ((DateTime.Now - HttpWebResponseProxyHuake.DtLastSuccessTime).TotalSeconds <= 60)
            {
                System.Threading.Thread.Sleep(3000);
            }
        }
        public static void Part2()
        {
            new Task(() =>
            {
                new CrawlerQuestionHugeHuake().Crawler();
            }).Start();
            while ((DateTime.Now - HttpWebResponseProxyHuake.DtLastSuccessTime).TotalSeconds <= 60)
            {
                System.Threading.Thread.Sleep(3000);
            }
        }

    }
}
