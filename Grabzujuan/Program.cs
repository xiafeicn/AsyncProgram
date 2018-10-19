using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHtmlUnit;
using NSoup;

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

            WebClient webClient = new WebClient(BrowserVersion.CHROME);
            webClient.CssEnabled = false;
            webClient.JavaScriptEnabled = true;
            var page = webClient.GetHtmlPage("https://www.zujuan.com/question?tree_type=category&chid=3&xd=2");
            webClient.WaitForBackgroundJavaScript(1000);
            var html = page.AsXml();
            webClient.Close();
            var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");

            var lis = qlist.GetElementsByTag("li");

            var jArray = new JArray();
            foreach (var li in lis)
            {
                JObject obj = new JObject();
                obj["id"] = li.Attr("data-qid");
                obj["question"] = li.GetElementsByClass("exam-q").Text;
                obj["type"] = li.Attr("exam-new");
                var answers = li.GetElementsByClass("op-item");
                var opArray = new JArray();
                foreach (var ans in answers)
                {
                    var item = new JObject();
                    item["out"] = ans.GetElementsByClass("op-item-nut").Text;
                    if (ans.GetElementsByTag("img").Count > 0)
                    {
                        item["meat"] = HttpUtility.UrlDecode(ans.GetElementsByClass("mathml").Attr("src")); ;
                    }
                    else
                    {
                        item["meat"] = ans.GetElementsByClass("op-item-meat").Text;
                       
                    }
                    

                    opArray.Add(item);
                }

                obj["option"] = opArray;
                jArray.Add(obj);
            }

            var str = JsonConvert.SerializeObject(jArray);
        }
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