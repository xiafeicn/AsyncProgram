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
            //            var html = GetRealHtmlOrogin("https://www.zujuan.com/question/index?chid=3&xd=1&tree_type=knowledge&page=2&per-page=10");
            var html = GetRealHtmlOrogin("https://www.zujuan.com/question?chid=3&xd=2&tree_type=knowledge");

            var str = JsonConvert.SerializeObject(shuxue(html));
        }
        static string GetHtmlOrogin(string url)
        {
            WebClient webClient = new WebClient(BrowserVersion.CHROME);
            webClient.CssEnabled = true;
            webClient.JavaScriptEnabled = true;
            //webClient.CookieManager = new CookieManager();
            //webClient.CookieManager.AddCookie(new Cookie("www.zujuan.com", "chid", "27e8704a451201531cc9941f6f3b709b7e13397751c04b090603ffdb0a56dfb9a:2:{i:0;s:4:\"chid\";i:1;s:1:\"2\";}"));
            var page = webClient.GetHtmlPage(url);

            webClient.WaitForBackgroundJavaScript(1000);
            var html = page.AsXml();
            webClient.Close();
            return html;
        }

        static string GetRealHtmlOrogin(string url)
        {
            WebClient webClient = new WebClient(BrowserVersion.CHROME);
            webClient.CssEnabled = true;
            webClient.JavaScriptEnabled = true;
            //webClient.CookieManager = new CookieManager();
            //webClient.CookieManager.AddCookie(new Cookie("www.zujuan.com", "chid", "27e8704a451201531cc9941f6f3b709b7e13397751c04b090603ffdb0a56dfb9a:2:{i:0;s:4:\"chid\";i:1;s:1:\"2\";}"));
            var page = webClient.GetHtmlPage(url);
            webClient.WaitForBackgroundJavaScript(1000);
            page = webClient.GetHtmlPage(url);
            var html = page.AsXml();
            webClient.Close();
            return html;
        }

        static string GetHtml(string url)
        {
            WebClient webClient = new WebClient(BrowserVersion.CHROME);
            webClient.CssEnabled = true;
            webClient.JavaScriptEnabled = true;
            //webClient.CookieManager = new CookieManager();
            //webClient.CookieManager.AddCookie(new Cookie("www.zujuan.com", "chid", "27e8704a451201531cc9941f6f3b709b7e13397751c04b090603ffdb0a56dfb9a:2:{i:0;s:4:\"chid\";i:1;s:1:\"2\";}"));
            var page = webClient.GetHtmlPage(url);
            var button = page.GetElementByClassName("item-list").GetElementsByTagName("a")[0] as HtmlAnchor;
            HtmlPage realpage = button.Click() as HtmlPage;
            webClient.WaitForBackgroundJavaScript(1000);
            var html = realpage.AsXml();
            webClient.Close();
            return html;
        }


        public static JArray carwler1(string html)
        {
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

            return jArray;

        }
        public static JArray yuwen(string html)
        {
            var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");

            var lis = qlist.GetElementsByTag("li");

            var jArray = new JArray();
            foreach (var li in lis)
            {
                JObject obj = new JObject();
                obj["id"] = li.Attr("data-qid");

                obj["type"] = li.GetElementsByClass("exam-new").Text
                    ;
                var exam_q = li.GetElementsByClass("exam-q");
                if (exam_q[0].GetElementsByTag("img").Count > 0)
                {
                    var imgUrl = exam_q[0].GetElementsByTag("img")[0].Attr("src");
                    var base64 = ConvertHttpImageToBase64(imgUrl);

                    obj["question"] = li.GetElementsByClass("exam-q").Html().Replace(imgUrl, base64);
                }
                var type = li.GetElementsByClass("exam-head-left")[0].GetElementsByTag("span");


                obj["tixing"] = type[0].Text();
                obj["tilei"] = type[1].Text();
                obj["nanyidu"] = type[2].Text();
                var answers = li.GetElementsByClass("exam-qlist");
                var opArray = new JArray();
                foreach (var ans in answers)
                {
                    var item = new JObject();
                    item["exam-q"] = ans.GetElementsByClass("exam-q").Html();

                    opArray.Add(item);
                }

                obj["option"] = opArray;
                jArray.Add(obj);
            }

            return jArray;
        }

        public static JArray shuxue(string html)
        {
            var qlist = NSoupClient.Parse(html).GetElementById("J_QuestionList");

            var lis = qlist.GetElementsByTag("li");

            var jArray = new JArray();
            foreach (var li in lis)
            {
                JObject obj = new JObject();
                obj["id"] = li.Attr("data-qid");

                if (li.GetElementsByClass("exam-q").Count > 0)
                {
                    var exam_q = li.GetElementsByClass("exam-q")[0];
                    if (exam_q.GetElementsByTag("img").Count > 0)
                    {
                        exam_q = ProcessHtmlImageElement(exam_q);


                    }
                    obj["question"] = exam_q.Html();
                }

                if (li.GetElementsByClass("exam-qlist").Count > 0)
                {
                    var qList = li.GetElementsByClass("exam-qlist")[0];
                    var qArray = new JArray();
                    foreach (var q in qList.GetElementsByClass("exam-q"))
                    {
                        var cq = ProcessHtmlImageElement(q);
                        var item = new JObject();
                        item["question_item"] = cq.Html();

                        qArray.Add(item);
                    }

                    obj["question_list"] = qArray;
                }

                if (li.GetElementsByClass("exam-s").Count > 0)
                {
                    var opItems = li.GetElementsByClass("exam-s")[0].GetElementsByClass("op-item");
                    var opArray = new JArray();
                    foreach (var ans in opItems)
                    {
                        var item = new JObject();
                        item["out"] = ans.GetElementsByClass("op-item-nut").Text;
                        var currentAns = ProcessHtmlImageElement(ans);
                        if (ans.GetElementsByTag("img").Count > 0)
                        {
                            item["meat"] = currentAns.GetElementsByTag("img").Attr("src");
                        }
                        else
                        {
                            item["meat"] = currentAns.GetElementsByClass("op-item-meat").Text;

                        }


                        opArray.Add(item);
                    }
                    obj["option"] = opArray;
                }



                obj["type"] = li.GetElementsByClass("exam-new").Text;

                var type = li.GetElementsByClass("exam-head-left")[0].GetElementsByTag("span");


                obj["tixing"] = type[0].Text();
                obj["tilei"] = type[1].Text();
                obj["nanyidu"] = type[2].Text();


                //var qelist = li.GetElementsByClass("exam-qlist");
                //var qArray = new JArray();
                //foreach (var q in qelist)
                //{
                //    var item = new JObject();
                //    item["exam-q"] = q.GetElementsByClass("exam-q").Html();

                //    qArray.Add(q);
                //}

                //obj["question"] = qArray;
                jArray.Add(obj);
            }

            return jArray;
        }


        public static Element ProcessHtmlImageElement(Element element)
        {
            if (element.GetElementsByAttribute("data-cke-saved-src").Count > 0)
            {
                //连接性的图片
                foreach (var math in element.GetElementsByAttribute("data-cke-saved-src"))
                {
                    var imgUrl = math.Attr("src");
                    var base64 = ConvertHttpImageToBase64(imgUrl);
                    var newImageUrl = "data:image/png;base64," + base64;

                    math.Attr("src", newImageUrl.Trim(new char[] { '"' }));


                }

            }

            //if (element.GetElementsByClass("mathml").Count > 0)
            //{
            //    foreach (var math in element.GetElementsByClass("mathml"))
            //    {

            //        math.Attr("src", HttpUtility.UrlDecode(math.Attr("src")));


            //    };
            //}
            return element;
        }

        public static string ConvertHttpImageToBase64(string imgUrl)
        {
            Image _image = Image.FromStream(System.Net.WebRequest.Create(imgUrl).GetResponse().GetResponseStream() ?? throw new InvalidOperationException());
            return ImgToBase64String(_image);
        }

        /// <summary>
        /// 图片 转为    base64编码的文本
        /// </summary>
        /// <param name="bmp">待转的Bitmap</param>
        /// <returns>转换后的base64字符串</returns>
        public static String ImgToBase64String(Image bmp)
        {
            String strbaser64 = String.Empty;
            var btarr = convertByte(bmp);
            strbaser64 = Convert.ToBase64String(btarr);

            return strbaser64;
        }

        /// <summary>
        /// Image转byte[]
        /// </summary>
        /// <param name="img">Img格式数据</param>
        /// <returns>byte[]格式数据</returns>
        public static byte[] convertByte(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
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