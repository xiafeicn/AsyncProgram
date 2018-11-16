using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common;
using Grabzujuan;
using NHtmlUnit;
using NHtmlUnit.Html;
using NHtmlUnit.Util;
using NSoup;
using NSoup.Nodes;

namespace CrawlerHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            //1
            //new CrawlerBook().InitBook();
            //2
            //new CrawlerCategory().InitCategory();
            var start = System.Configuration.ConfigurationManager.AppSettings["START"].NullToInt();
            var end = System.Configuration.ConfigurationManager.AppSettings["END"].NullToInt();
            new CrawlerCategoryUrllist().InitCatePageUrl(start, end);
        }
    }
}

