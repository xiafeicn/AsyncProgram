using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Grabzujuan
{
    public class WebClientEx : WebClient
    {
        public int Timeout { get; set; } = 20000;

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }
    }
    public static class HttpClientHolder
    {
        public static string Proxy_GetRequest(string url, string ip_port)
        {
            try
            {
                WebClientEx webClient = new WebClientEx();

                webClient.Encoding = Encoding.GetEncoding("gb2312");
                if (true)
                {
                    WebProxy proxy = new WebProxy();
                    proxy.UseDefaultCredentials = false;
                    proxy.Address = new Uri($"http://{ip_port}");
                    webClient.Proxy = proxy;
                }

                Stream stream = webClient.OpenRead(url);

                StreamReader sr = new StreamReader(stream);
                var result = sr.ReadToEnd();

                Debug.WriteLine(url);
                return result;
            }
            catch (IOException ioException)
            {
                throw;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    switch (((System.Net.HttpWebResponse)(ex.Response)).StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException("UnauthorizedAccess", ex);
                        default:
                            throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {

            }
        }
        public static string Proxy_GetRequest2(string url)
        {
            try
            {
                WebClientEx webClient = new WebClientEx();

                webClient.Encoding = Encoding.GetEncoding("gb2312");
                if (true)
                {
                    //IP: 帐号: tets1106密码: tets1106开通成功！http端口：808
                    //创建 代理服务器设置对象 的实例
                    System.Net.WebProxy wp = new System.Net.WebProxy("123.249.2.2:888");
                    //代理服务器需要验证
                    wp.BypassProxyOnLocal = false;
                    //用户名密码
                    wp.Credentials = new NetworkCredential("te1107", "te1107");
                    ////将代理服务器设置对象赋予全局设定
                    //System.Net.GlobalProxySelection.Select = wp;


                   
                    webClient.Proxy = wp;
                }
                webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
                webClient.Headers.Add("cookie", "_ga=GA1.2.1590408373.1540594397; _gid=GA1.2.444123411.1542285873; isRemove=1; chid=27e8704a451201531cc9941f6f3b709b7e13397751c04b090603ffdb0a56dfb9a%3A2%3A%7Bi%3A0%3Bs%3A4%3A%22chid%22%3Bi%3A1%3Bs%3A1%3A%222%22%3B%7D; xd=ff8cc2c663e498cf1fffa3d89aaa8ae9f68a128de39a6036c46ec0a0ff0b9459a%3A2%3A%7Bi%3A0%3Bs%3A2%3A%22xd%22%3Bi%3A1%3Bs%3A1%3A%221%22%3B%7D; _csrf=1398d38e93448214bc3159f11f82c6cabe64e6424474799b7e13cf35c027f7e9a%3A2%3A%7Bi%3A0%3Bs%3A5%3A%22_csrf%22%3Bi%3A1%3Bs%3A32%3A%227Q3dvZPJu0TCJGVZskyZO_BCYDzkDojd%22%3B%7D; Hm_lvt_6de0a5b2c05e49d1c850edca0c13051f=1541849628,1542198947,1542285873,1542291479; Hm_lpvt_6de0a5b2c05e49d1c850edca0c13051f=1542291797");
                Stream stream = webClient.OpenRead(url);

                StreamReader sr = new StreamReader(stream);
                var result = sr.ReadToEnd();

                Debug.WriteLine(url);
                return result;
            }
            catch (IOException ioException)
            {
                throw;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    switch (((System.Net.HttpWebResponse)(ex.Response)).StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException("UnauthorizedAccess", ex);
                        default:
                            throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {

            }
        }
        public static string Proxy_GetRequestAbyyun(string url)
        {
            try
            {
                WebClientEx webClient = new WebClientEx();

                webClient.Encoding = Encoding.GetEncoding("gb2312");
                if (true)
                {
                    //WebProxy proxy = new WebProxy();
                    //proxy.UseDefaultCredentials = true;
                    //proxy.Address = new Uri($"http://{ip_port}");
                    //webClient.Proxy = proxy;
                    //proxy.Credentials=new bas
                    ////////webClient.
                    ////////HttpTransportProperties.Authenticator basicauth = new HttpTransportProperties.Authenticator();
                    ////////basicauth.setUsername("username");  //服务器访问用户名 
                    ////////basicauth.setPassword("password"); //服务器访问密码
                    ////////options.setProperty(HTTPConstants.AUTHENTICATE, basicauth);
                    ////////---------------------
                    ////////作者：Jimstin
                    ////////来源：CSDN
                    ////////原文：https://blog.csdn.net/zhangjm_123/article/details/26581971 
                    // 代理服务器
                    String proxyServer = "http-dyn.abuyun.com";
                    int proxyPort = 9020;

                    // 代理隧道验证信息
                    String proxyUser = "H25D3VA48R0D97AD";
                    String proxyPass = "68456EC7231F86C3";

                    //Authenticator.setDefault(new ProxyAuthenticator(proxyUser, proxyPass));
                    //webClient.Credentials = GetCredentialCache("http-dyn.abuyun.com", proxyUser, proxyPass);

                    webClient.Headers.Add("Proxy-Authorization", GetAuthorization(proxyUser, proxyPass));
                }

                Stream stream = webClient.OpenRead(url);

                StreamReader sr = new StreamReader(stream);
                var result = sr.ReadToEnd();

                Debug.WriteLine(url);
                return result;
            }
            catch (IOException ioException)
            {
                throw;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    switch (((System.Net.HttpWebResponse)(ex.Response)).StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException("UnauthorizedAccess", ex);
                        default:
                            throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {

            }
        }
        public static string GetRequest(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                //webClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");

                //webClient.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                //webClient.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en;q=0.6");
                //webClient.Headers.Add("Cache-Control", "max-age=0");
                //webClient.Headers.Add("Connection", "keep-alive");
                //webClient.Headers.Add("Host", "movie.douban.com");
                webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");

                Stream stream = webClient.OpenRead(url);
                StreamReader sr = new StreamReader(stream);
                var result = sr.ReadToEnd();
                Debug.WriteLine(url);


                return result;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    switch (((System.Net.HttpWebResponse)(ex.Response)).StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException("UnauthorizedAccess", ex);
                        default:
                            throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {

            }
        }

        #region # 生成 Http Basic 访问凭证 #

        private static CredentialCache GetCredentialCache(string uri, string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);

            CredentialCache credCache = new CredentialCache();
            credCache.Add(new Uri(uri), "Basic", new NetworkCredential(username, password));

            return credCache;
        }

        private static string GetAuthorization(string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);

            return "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(authorization));
        }

        #endregion

        public static string GetResponseCookie(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(url);
                StreamReader sr = new StreamReader(stream);
                var result = sr.ReadToEnd();
                Debug.WriteLine(url);


                return webClient.ResponseHeaders["Set-Cookie"];
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    switch (((System.Net.HttpWebResponse)(ex.Response)).StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException("UnauthorizedAccess", ex);
                        default:
                            throw;
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {

            }
        }

        public static string Execute(string url, string cookie)
        {
            List<Exception> e = new List<Exception>();
            int retry = 5;
            for (var i = 0; i < retry; i++)
            {
                bool hasException = false;
                try
                {
                    return GetRequest(url, cookie);
                }
                catch (Exception ex)
                {
                    e.Add(ex);
                    hasException = true;
                }
                if (hasException)
                {
                    Task.Delay(1000);
                }
            }
            return string.Empty;
        }


        public static string Execute(string url)
        {
            List<Exception> e = new List<Exception>();
            int retry = 5;
            for (var i = 0; i < retry; i++)
            {
                bool hasException = false;
                try
                {
                    return GetRequest(url);
                }
                catch (Exception ex)
                {
                    e.Add(ex);
                    hasException = true;
                }
                if (hasException)
                {
                    Task.Delay(1000);
                }
            }
            return string.Empty;
        }
        public static string GetRequest(string url, string cookie)
        {
            var result = string.Empty;
            WebClient webClient = new WebClient();

            webClient.Headers.Add(HttpRequestHeader.Cookie, cookie);
            Stream stream = webClient.OpenRead(url);
            StreamReader sr = new StreamReader(stream);
            result = sr.ReadToEnd();
            Debug.WriteLine(url);
            return result;
        }
        public static string Post(string url, string postData)
        {
            byte[] bs = Encoding.ASCII.GetBytes(postData);
            var req = (System.Net.HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 90000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            req.AllowAutoRedirect = false;

            req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public static void GetPost(string url, string postData)
        {
            try
            {
                WebClient client = new WebClient();
                byte[] sendData = Encoding.GetEncoding("utf-8").GetBytes(postData);
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Headers.Add("ContentLength", sendData.Length.ToString());
                byte[] recData = client.UploadData(url, "POST", sendData);
            }
            catch (WebException)
            {
                throw;
            }
        }

        public static string GetRequestForExport(string url)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                Stream stream = client.OpenRead(url);
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (WebException)
            {
                return "0";
            }
        }
    }
}
