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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace CrawlerXKW
{
    public class CrawlerSubject
    {
        /// <summary>
        /// http://zujuan.xkw.com/gzyw/zj75639/?action=getcategory  获取所有教材版本
        /// </summary>
        public void Crawler()
        {
            //此时记得添加路径    
            using (var driver = new ChromeDriver())
            {
                //进入百度首页
                driver.Navigate().GoToUrl(@"http://zujuan.xkw.com/gzyw/zj75639/");
                driver.Manage().Window.Minimize();
                var html = driver.PageSource;

                var all = JArray.Parse(StringExtensions.GetValue(html, "var edu=", ";var diffs="));
                var areas = JArray.Parse(StringExtensions.GetValue(html, "var province=", ";var answerimg"));
                foreach (var degree in all)
                {
                    foreach (var subject in JArray.Parse(degree["QuesBankList"].ToString()))
                    {
                        var subjectId = subject["ID"].NullToInt();
                        var subjectName = subject["Name"].ToString();
                        var degreeid = subject["EduID"].NullToInt();
                        AddSubject(subjectId, subjectName, degreeid);

                        foreach (var category in JArray.Parse(subject["CategoryList"].ToString()))
                        {
                            var categoryId = category["ID"].NullToInt();
                            var categoryName = category["Name"].ToString();
                            AddCategoryId(categoryId, categoryName, subjectId);

                        }
                        foreach (var grade in JArray.Parse(subject["LearnGradeList"].ToString()))
                        {
                            var gradeId = grade["ID"].NullToInt();
                            AddSubjectGrade(gradeId, subjectId);
                        }


                    }
                }

                foreach (var area in areas)
                {
                    var areaid = area["ID"].NullToInt();
                    var name = area["Name"].ToString();
                    var shortName = area["ShortName"].ToString();
                    AddArea(areaid, name, shortName);
                }
                //Thread.Sleep(1000);

                ////设置固定宽，高
                //driver.Manage().Window.Size = new System.Drawing.Size(100, 200);
                //Thread.Sleep(1000);

                ////设置窗体最大化
                //driver.Manage().Window.Maximize();
                //Thread.Sleep(1000);

                ////找到对象
                //var searchBox = driver.FindElementById("kw1");
                //var btnClick = driver.FindElementById("su1");

                ////发送搜索内容
                //searchBox.SendKeys("selenium");
                //Thread.Sleep(1000);

                ////点击按钮
                //btnClick.Click();
                //Thread.Sleep(1000);

                ////后退到百度首页
                //driver.Navigate().Back();
                //Thread.Sleep(1000);

                ////回到新闻页
                //driver.Navigate().Forward();
                //Thread.Sleep(1000);

                ////截图
                ////自动化测试中截图的图片用当前时间来命名，会起到非常不错的效果
                //string pictrueName = DateTime.Now.ToString();
                //if (pictrueName.Contains(':'))
                //{
                //    pictrueName = pictrueName.Replace(':', '_');
                //}
                //if (pictrueName.Contains('/'))
                //{
                //    pictrueName = pictrueName.Replace('/', '_');
                //}

                //driver.GetScreenshot().SaveAsFile(@"D:\" + pictrueName + ".Jpeg", ImageFormat.Jpeg);
                //Thread.Sleep(1000);

                //退出
                driver.Quit();
            }
        }

        public void AddSubject(int subjectId, string subjectName, int degreeId)
        {
            using (var db = new XKWEntities2())
            {
                if (!db.Subject.Any(t => t.SubjectId == subjectId))
                {
                    var entity = new Subject();
                    entity.SubjectId = subjectId;
                    entity.SubjectName = subjectName;
                    entity.DegreeId = degreeId;
                    db.Subject.Add(entity);
                    db.SaveChanges();
                }
            }
        }
        public void AddCategoryId(int categoryId, string categoryName, int subjectId)
        {
            using (var db = new XKWEntities2())
            {
                if (!db.SubjectCategory.Any(t => t.CategoryId == categoryId))
                {
                    var entity = new SubjectCategory();
                    entity.CategoryId = categoryId;
                    entity.CategoryName = categoryName;
                    entity.SubjectId = subjectId;
                    db.SubjectCategory.Add(entity);
                    db.SaveChanges();
                }
            }
        }

        public void AddSubjectGrade(int gradeId, int subjectId)
        {
            using (var db = new XKWEntities2())
            {
                if (!db.SubjectGrade.Any(t => t.SubjectId == subjectId && t.GradeId == gradeId))
                {
                    var entity = new SubjectGrade();
                    entity.GradeId = gradeId;
                    entity.SubjectId = subjectId;
                    db.SubjectGrade.Add(entity);
                    db.SaveChanges();
                }
            }
        }

        public void AddArea(int areaId, string name, string shortName)
        {
            using (var db = new XKWEntities2())
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
