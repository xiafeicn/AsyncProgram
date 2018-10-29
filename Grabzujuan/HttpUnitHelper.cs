using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.net;
using NHtmlUnit;
using NHtmlUnit.Html;

namespace Grabzujuan
{
    public class HttpUnitHelper
    {

        static HttpUnitHelper()
        {
            wc.CssEnabled = true;
            wc.JavaScriptEnabled = true;
        }
        public static WebClient wc = new WebClient(BrowserVersion.CHROME);

        public WebClient webClient
        {
            get
            {
                return wc;
            }
        }

        public void Login(string user, string password)
        {
            var page = wc.GetPage("https://passport.zujuan.com/login") as HtmlPage;
            //登录

            HtmlInput ln = page.GetHtmlElementById("user-name") as HtmlInput;
            HtmlInput pwd = page.GetHtmlElementById("user-pwd") as HtmlInput;
            HtmlButton btn = page.GetFirstByXPath("/html/body/div[1]/div[1]/form/div[1]/div[3]/button") as HtmlButton;
            ln.SetAttribute("value", user);
            pwd.SetAttribute("value", password);

            HtmlPage page2 = btn.Click() as HtmlPage;
            //todo check login status
            //登录完成，现在可以爬取任意你想要的页面了。
        }

        /// <summary>
        /// 组卷网第二次打开网页才能得到数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetRealHtmlTrice(string url)
        {
            var page = webClient.GetHtmlPage(url);
            webClient.WaitForBackgroundJavaScript(1000);
            page = webClient.GetHtmlPage(url);
            var html = page.AsXml();
            webClient.Close();
            return html;
        }

        public string GetRealHtmlOnce(string url)
        {
            var page = webClient.GetHtmlPage(url);
            webClient.WaitForBackgroundJavaScript(1000);
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
    }
}
