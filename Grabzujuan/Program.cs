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
        static void Main(string[] args)
        {
            // NSoup.Nodes.Document doc = NSoup.NSoupClient.Connect("https://www.zujuan.com/question?tree_type=category&chid=3&xd=2").Get();
            //var html = NSoupClient.Connect("https://www.zujuan.com/question?tree_type=category&chid=3&xd=2").Execute().Body();
            //var html = HttpClientHolder.GetRequest("https://www.zujuan.com/question?tree_type=category&chid=3&xd=2");

            //var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");


            //var html = GetHtml("https://www.zujuan.com/question?tree_type=category&chid=3&xd=2");
            //var html = GetHtml("https://www.zujuan.com/question?chid=2&xd=1&tree_type=knowledge&_=1313");

            //var str = JsonConvert.SerializeObject(yuwen(html));
            //            var html = GetRealHtmlTrice("https://www.zujuan.com/question/index?chid=3&xd=1&tree_type=knowledge&page=2&per-page=10");
            //var html = HttpUnitHelper.GetRealHtmlTrice("https://www.zujuan.com/question?chid=3&xd=2&tree_type=knowledge");

            //new HttpUnitHelper().Login("13661614607", "123456");

            //第一级菜单
            // var html = new HttpUnitHelper().GetRealHtmlTrice("https://www.zujuan.com/question?chid=2&xd=1&tree_type=knowledge");

            //var total = NSoupClient.Parse(html).GetElementsByClass(".total")[0].GetElementsByTag("b")[0].Text().NullToInt();

            //https://www.zujuan.com/question/index?chid=2&xd=1&tree_type=knowledge&page=2&per-page=10
            //var html = new HttpUnitHelper().GetRealHtmlTrice("https://www.zujuan.com/question?chid=3&xd=2&tree_type=knowledge");
            //var str = JsonConvert.SerializeObject(new QuestionParser().ParseMath(html));
            new HttpUnitHelper().Login("13661614607", "123456");
            var j1 = new QuestionParser().ParseAnswer("8848576");
            var j2 = new QuestionParser().ParseAnswer("8764862");
            //var aa=new QuestionParser().ParseAnswer("");
        }





        //public static JArray carwler1(string html)
        //{
        //    var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");

        //    var lis = qlist.GetElementsByTag("li");

        //    var jArray = new JArray();
        //    foreach (var li in lis)
        //    {
        //        JObject obj = new JObject();
        //        obj["id"] = li.Attr("data-qid");
        //        obj["question"] = li.GetElementsByClass("exam-q").Text;
        //        obj["type"] = li.Attr("exam-new");
        //        var answers = li.GetElementsByClass("op-item");
        //        var opArray = new JArray();
        //        foreach (var ans in answers)
        //        {
        //            var item = new JObject();
        //            item["out"] = ans.GetElementsByClass("op-item-nut").Text;
        //            if (ans.GetElementsByTag("img").Count > 0)
        //            {
        //                item["meat"] = HttpUtility.UrlDecode(ans.GetElementsByClass("mathml").Attr("src")); ;
        //            }
        //            else
        //            {
        //                item["meat"] = ans.GetElementsByClass("op-item-meat").Text;

        //            }


        //            opArray.Add(item);
        //        }

        //        obj["option"] = opArray;
        //        jArray.Add(obj);
        //    }

        //    return jArray;

        //}
        //public static JArray yuwen(string html)
        //{
        //    var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");

        //    var lis = qlist.GetElementsByTag("li");

        //    var jArray = new JArray();
        //    foreach (var li in lis)
        //    {
        //        JObject obj = new JObject();
        //        obj["id"] = li.Attr("data-qid");

        //        obj["type"] = li.GetElementsByClass("exam-new").Text
        //            ;
        //        var exam_q = li.GetElementsByClass("exam-q");
        //        if (exam_q[0].GetElementsByTag("img").Count > 0)
        //        {
        //            var imgUrl = exam_q[0].GetElementsByTag("img")[0].Attr("src");
        //            var base64 = ConvertHttpImageToBase64(imgUrl);

        //            obj["question"] = li.GetElementsByClass("exam-q").Html().Replace(imgUrl, base64);
        //        }
        //        var type = li.GetElementsByClass("exam-head-left")[0].GetElementsByTag("span");


        //        obj["tixing"] = type[0].Text();
        //        obj["tilei"] = type[1].Text();
        //        obj["nanyidu"] = type[2].Text();
        //        var answers = li.GetElementsByClass("exam-qlist");
        //        var opArray = new JArray();
        //        foreach (var ans in answers)
        //        {
        //            var item = new JObject();
        //            item["exam-q"] = ans.GetElementsByClass("exam-q").Html();

        //            opArray.Add(item);
        //        }

        //        obj["option"] = opArray;
        //        jArray.Add(obj);
        //    }

        //    return jArray;
        //}




    }
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