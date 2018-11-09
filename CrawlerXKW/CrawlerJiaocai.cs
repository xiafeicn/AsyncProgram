using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Grabzujuan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSoup;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace CrawlerXKW
{
    public class CrawlerJiaocai
    {
        public void Crawler()
        {
            using (var driver = new ChromeDriver())
            {
                //进入百度首页
                driver.Navigate().GoToUrl(@"http://zujuan.xkw.com/gzyw/zj75639/g11/?action=getcategory");
                driver.Manage().Window.Minimize();
                var html = driver.PageSource;
                var doc = NSoupClient.Parse(html);
            }
        }


        public void AddArea(int areaId, string name, string shortName)
        {
            using (var db = new XKWEntities())
            {
                if (!db.Area.Any(t => t.AreaId == areaId))
                {
                    var entity = new Area();
                    entity.AreaId = areaId;
                    entity.Name = name;
                    entity.ShortName = shortName;
                    db.Area.Add(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
