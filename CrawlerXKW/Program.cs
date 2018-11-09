using System;
using System.Collections.Generic;
using System.Text;
using Grabzujuan;

namespace CrawlerXKW
{
    class Program
    {
        static void Main(string[] args)
        {
            //new CrawlerSubject().Crawler();
            //new CrawlerJiaocai().Crawler();

            var res = HttpClientHolder.GetRequest("http://zujuan.xkw.com/gzyw/zj75639/");

        }

      
    }
}
