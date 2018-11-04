using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Common;
using java.math;
using java.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHtmlUnit;
using NHtmlUnit.Html;
using NHtmlUnit.Util;
using NSoup;
using NSoup.Nodes;

namespace Grabzujuan
{
    class Program
    {
        private JObject result = new JObject();

        static Dictionary<int, List<string>> dicQuestion = new Dictionary<int, List<string>>();
        static void Main(string[] args)
        {

            var res = HttpClientHolder.GetRequest("https://www.zujuan.com/question?chid=2&xd=1&tree_type=knowledge");
            var doc = NSoupClient.Parse(res).GetElementsByClass("item-list")[0].GetElementsByTag("a");

            foreach (var href in doc)
            {
                var url = "https://www.zujuan.com"+ href.Attr("href");

                new HttpUnitHelper().GetRealHtmlTrice(url);

                var cooks = new HttpUnitHelper().webClient.GetCookies(new URL("https://www.zujuan.com"));
                var xd = cooks.Where(t => t.Name == "xd").FirstOrDefault().Value;
                var chid = cooks.Where(t => t.Name == "chid").FirstOrDefault().Value;
                //var aa = HttpClientHolder.GetResponseCookie("https://www.zujuan.com" + url);
                //var a = HttpClientHolder.GetResponseCookie("https://www.zujuan.com" + url);
                //var xd = GrabAnswers.GetValue(a, "xd=", "path=/;");
                //var chid = GrabAnswers.GetValue(a, "chid=", "path=/;");

                using (var db = new CrawlerEntities())
                {
                    var table = new CookieState();
                    table.Xd = StringExtensions.GetQueryString("xd", url).NullToInt();
                    table.Chid = StringExtensions.GetQueryString("chid", url).NullToInt();
                    table.Cookie =
                        $"xd={xd} chid={chid} ";
                    db.CookieState.Add(table);
                    db.SaveChanges();
                }
            }

            // //
            // //var a = ques.Take(100).Select(t => JObject.Parse(t.ApiJson)).SelectMany(t => t["data"]).SelectMany(t => t["questions"]).Select(t => t["question_id"].ToString());

            // //
            // //var j1 = new QuestionParser().ParseAnswer("8848576");
            // //var j2 = new QuestionParser().ParseAnswer("8764862");
            // //var a1 = j1.ToString();
            // //var a2 = j2.ToString();
            // //new HttpUnitHelper().webClient.CookieManager.ClearCookies();

            // //Console.WriteLine("value的值为：{0}", value);
            // //var res = new HttpUnitHelper().GetRealHtmlOnceNotWaitJs("https://www.zujuan.com/question/detail-8788506.shtml");
            // //var queslist = DataService.GetQuestionList();
            // //new HttpUnitHelper().Login("13661614607", "123456");
            // //foreach (var que in queslist)
            // //{
            // //    ProcessPageQuestion(que);
            // //}


            //// GrabAnswer.GetProxyListFromBy();
            //  GrabAnswers.StartSync();
            // System.Windows.Forms.Application.Restart();
        }
        //new CategoryCrawler().InitBook();




    }
}

public class QuestionItem
{

    public int QuestionId { get; set; }
    public int PaperId { get; set; }

    public int exam_type { get; set; }
    public int grade_id { get; set; }
    public int kid_num { get; set; }
    public string question_text { get; set; }
    public int difficult_index { get; set; }
    public int question_channel_type { get; set; }
    public string apiJson { get; set; }
}


//final WebClient webClient=new WebClient(BrowserVersion.CHROME);
//String url2 = "URL";
//System.out.println(" // 1 启动JS ");
//webClient.getOptions().setJavaScriptEnabled(true);
//System.out.println("// 2 禁用Css，可避免自动二次请求CSS进行渲染 ");
//webClient.getOptions().setCssEnabled(false);
//System.out.println("// 3 启动客户端重定向 ");
//webClient.getOptions().setRedirectEnabled(true);
//System.out.println("// 4 js运行错误时，是否抛出异常");
//webClient.getOptions().setThrowExceptionOnScriptError(false);
//System.out.println("// 5 设置超时 ");
//webClient.getOptions().setTimeout(50000);
//System.out.println("  允许绕过SSL认证 ");
//webClient.getOptions().setUseInsecureSSL(true);
//System.out.println("  允许启动注册组件 ");
//webClient.getOptions().setActiveXNative(true);
//HtmlPage page = webClient.getPage(url);
//System.out.println(" //等待JS驱动dom完成获得还原后的网页 ");
//webClient.waitForBackgroundJavaScript(10000);  
//System.out.println("  网页内容 ");
//System.out.println(page.asXml());
//webClient.closeAllWindows();  
//--------------------- 