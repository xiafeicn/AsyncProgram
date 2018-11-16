using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Grabzujuan;
using Grabzujuan.Common;
using Newtonsoft.Json.Linq;

namespace Common.Common
{
    /// <summary>  
    /// 有关HTTP请求的辅助类  
    /// </summary>  
    public class HttpWebResponseUtility
    {

        private static object objlock = new object();

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public HttpWebResponseUtility()
        {
        }

        public static string ExecuteCreateGetHttpResponseProxy(string url, int? timeout, string cookie)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start(); ;

            List<Exception> e = new List<Exception>();
            int retry = 3;
            for (var i = 0; i < retry; i++)
            {
                bool hasException = false;
                try
                {
                    var userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
                    //#if DEBUG

                    //                    Debug.WriteLine("start" + url);
                    //                    Console.WriteLine("start" + url);
                    //#endif
                    System.Net.HttpWebRequest request = WebRequest.Create(url) as System.Net.HttpWebRequest;
                    request.Method = "GET";
                    request.UserAgent = DefaultUserAgent;
                    if (!string.IsNullOrEmpty(userAgent))
                    {
                        request.UserAgent = userAgent;
                    }
                    if (timeout.HasValue)
                    {
                        request.Timeout = timeout.Value;
                    }
                    //if (cookies != null)
                    //{
                    //    request.CookieContainer = new CookieContainer();
                    //    request.CookieContainer.Add(cookies);
                    //}
                    request.Headers["cookie"] = cookie;

                    fillProxyMogu(request);

                    var MyResponse = request.GetResponse() as HttpWebResponse;

                    string strReturn = string.Empty;
                    Stream stream = MyResponse.GetResponseStream();

                    var encoding = Encoding.GetEncoding("utf-8");
                    // 如果要下载的页面经过压缩，则先解压
                    if (MyResponse.ContentEncoding.ToLower().IndexOf("gzip") >= 0)
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    if (encoding == null)
                    {
                        encoding = Encoding.Default;
                    }
                    strReturn += new StreamReader(stream, encoding).ReadToEnd();//解决乱码：utf-8 + streamreader.readtoend

                    if (strReturn.IndexOf("拒绝访问") >= 0)
                    {
                        Console.WriteLine("invalid proxy");
                        throw new Exception("invalid proxy");
                    }
                    if (strReturn.IndexOf("涉嫌恶意操作") >= 0)
                    {
                        Console.WriteLine("invalid proxy");
                        throw new Exception("invalid proxy");
                    }
                    //if (strReturn.IndexOf("questioncount") < 0)
                    //{
                    //    throw new Exception("invalid html result");
                    //}
#if DEBUG
                    stopWatch.Stop();
                    Debug.WriteLine($"cost {stopWatch.ElapsedMilliseconds} ms,url{url}");
                    Console.WriteLine($"cost {stopWatch.ElapsedMilliseconds} ms,url{url}");
                    Debug.WriteLine("success      " + url);
                    Console.WriteLine("success       " + url);
#endif
                    return strReturn;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("proxy out time");
                    throw new Exception("proxy out time");
                    e.Add(ex);
                    hasException = true;
                }
                if (hasException)
                {
                    Task.Delay(1000);
                }
            }

          
            return String.Empty;
        }

        public static string ExecuteCreateGetHttpResponse(string url, int? timeout, CookieCollection cookies)
        {
            List<Exception> e = new List<Exception>();
            int retry = 10;
            for (var i = 0; i < retry; i++)
            {
                bool hasException = false;
                try
                {
                    var userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
#if DEBUG

                    Debug.WriteLine(url);
#endif
                    System.Net.HttpWebRequest request = WebRequest.Create(url) as System.Net.HttpWebRequest;
                    request.Method = "GET";
                    request.UserAgent = DefaultUserAgent;
                    if (!string.IsNullOrEmpty(userAgent))
                    {
                        request.UserAgent = userAgent;
                    }
                    if (timeout.HasValue)
                    {
                        request.Timeout = timeout.Value;
                    }
                    if (cookies != null)
                    {
                        request.CookieContainer = new CookieContainer();
                        request.CookieContainer.Add(cookies);
                    }

                    var MyResponse = request.GetResponse() as HttpWebResponse;

                    string strReturn = string.Empty;
                    Stream stream = MyResponse.GetResponseStream();

                    var encoding = Encoding.GetEncoding("utf-8");
                    // 如果要下载的页面经过压缩，则先解压
                    if (MyResponse.ContentEncoding.ToLower().IndexOf("gzip") >= 0)
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    if (encoding == null)
                    {
                        encoding = Encoding.Default;
                    }
                    strReturn += new StreamReader(stream, encoding).ReadToEnd();//解决乱码：utf-8 + streamreader.readtoend
                    return strReturn;
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
            return String.Empty;
        }


        /// <summary>
        /// 填充代理
        /// </summary>
        /// <param name="proxyUri"></param>
        private static void fillProxy(HttpWebRequest request)
        {
            var proxys = GetProxyListFromCache();
            var proxy = proxys[new Random().Next(0, proxys.Count - 1)];
            //IP: 帐号: tets1106密码: tets1106开通成功！http端口：808
            //创建 代理服务器设置对象 的实例
            System.Net.WebProxy wp = new System.Net.WebProxy(proxy);
            //代理服务器需要验证
            wp.BypassProxyOnLocal = false;
            //用户名密码
            //wp.Credentials = new NetworkCredential("te1107", "te1107");



            request.Proxy = wp;
        }

        private static void fillProxyMogu(HttpWebRequest request)
        {
            var proxys = GetMoguProxyListFromCache();
            var proxy = proxys[new Random().Next(0, proxys.Count - 1)];
            //IP: 帐号: tets1106密码: tets1106开通成功！http端口：808
            //创建 代理服务器设置对象 的实例
            System.Net.WebProxy wp = new System.Net.WebProxy(proxy);
            //代理服务器需要验证
            wp.BypassProxyOnLocal = false;
            //用户名密码
            //wp.Credentials = new NetworkCredential("te1107", "te1107");



            request.Proxy = wp;
        }
        public static string ExecuteCreateGetHttpResponseProxy2(string url, int? timeout, string cookie)
        {
            List<Exception> e = new List<Exception>();
            int retry = 3;

            for (var i = 0; i < retry; i++)
            {
                ProxyManager.HasExpire = false;
                bool hasException = false;
                try
                {
                    var userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
                    //#if DEBUG

                    //                    Debug.WriteLine("start" + url);
                    //                    Console.WriteLine("start" + url);
                    //#endif
                    System.Net.HttpWebRequest request = WebRequest.Create(url) as System.Net.HttpWebRequest;
                    request.Method = "GET";
                    request.UserAgent = DefaultUserAgent;
                    if (!string.IsNullOrEmpty(userAgent))
                    {
                        request.UserAgent = userAgent;
                    }
                    if (timeout.HasValue)
                    {
                        request.Timeout = timeout.Value;
                    }
                    //if (cookies != null)
                    //{
                    //    request.CookieContainer = new CookieContainer();
                    //    request.CookieContainer.Add(cookies);
                    //}
                    request.Headers["cookie"] = cookie;

                    fillProxy2(request);

                    var MyResponse = request.GetResponse() as HttpWebResponse;

                    string strReturn = string.Empty;
                    Stream stream = MyResponse.GetResponseStream();

                    var encoding = Encoding.GetEncoding("utf-8");
                    // 如果要下载的页面经过压缩，则先解压
                    if (MyResponse.ContentEncoding.ToLower().IndexOf("gzip") >= 0)
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    if (encoding == null)
                    {
                        encoding = Encoding.Default;
                    }
                    strReturn += new StreamReader(stream, encoding).ReadToEnd();//解决乱码：utf-8 + streamreader.readtoend

                    if (strReturn.IndexOf("拒绝访问") >= 0)
                    {
                        ProxyManager.HasExpire = true;
                        Console.WriteLine("invalid proxy");
                        throw new Exception("invalid proxy");
                    }
                    if (strReturn.IndexOf("涉嫌恶意操作") >= 0)
                    {
                        ProxyManager.HasExpire = true;
                        Console.WriteLine("invalid proxy");
                        throw new Exception("invalid proxy");
                    }
                    if (strReturn.IndexOf("questioncount") < 0)
                    {
                        throw new Exception("invalid html result");
                    }
#if DEBUG

                    Debug.WriteLine("success      " + url);
                    Console.WriteLine("success       " + url);
#endif
                    return strReturn;
                }
                catch (Exception ex)
                {

                    Console.WriteLine("proxy out time");
                    throw new Exception("proxy out time");
                    e.Add(ex);
                    hasException = true;
                }
                if (hasException)
                {
                    Task.Delay(1000);
                }
            }
            return String.Empty;
        }
        /// <summary>
        /// 填充代理
        /// </summary>
        /// <param name="proxyUri"></param>
        private static void fillProxy2(HttpWebRequest request)
        {
            var proxy = ProxyManager.GetProxy();

            //IP: 帐号: tets1106密码: tets1106开通成功！http端口：808
            //创建 代理服务器设置对象 的实例
            System.Net.WebProxy wp = new System.Net.WebProxy(proxy);
            //代理服务器需要验证
            wp.BypassProxyOnLocal = false;
            Console.WriteLine(proxy);
            request.Proxy = wp;
        }

        public static List<string> GetProxyListFromCache()
        {
            var fullName = MemoryCacheHelper.GetCacheItem<List<string>>("proxyList",
               delegate () { return GetProxyListFromBuy(); },
               new TimeSpan(0, 0, 30));//30分钟过期
            return fullName;

        }
        public static List<string> GetMoguProxyListFromCache()
        {
            var fullName = MemoryCacheHelper.GetCacheItem<List<string>>("moguproxyList",
                delegate ()
                {
                    var res = HttpClientHolder.GetRequest("http://piping.mogumiao.com/proxy/api/get_ip_bs?appKey=e7178154a26948f38f155af1e4f7a440&count=5&expiryDate=0&format=1&newLine=2");

                    List<string> listProxy = new List<string>();

                    var jarray = JObject.Parse(res)["msg"] as JArray;
                    foreach (var jitem in jarray)
                    {
                        listProxy.Add($"{jitem["ip"].NullToString()}:{jitem["port"].NullToString()}");
                    }

                    return listProxy;
                },
               new TimeSpan(0, 0, 20));//30分钟过期
            return fullName;

        }
        public static List<string> UnusefulProxy = new List<string>();

        public static List<string> GetProxyListFromBuy()
        {
            //var res = HttpClientHolder.GetRequest("http://www.flnsu.com/ip.php?key=6353621586&tqsl=1000");
            var res = HttpClientHolder.GetRequest("http://dev.kdlapi.com/api/getproxy/?orderid=934190848762936&num=500&quality=1");

            var listProxy = res.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            //var listUsefulProxy = new List<string>();
            //Parallel.ForEach(listProxy, (proxy) =>
            //{
            //    if (listUsefulProxy.Count > 30)
            //        return;
            //    var ip = proxy.Substring(0, proxy.IndexOf(":"));
            //    if (IsProxyValid(ip))
            //    {
            //        listUsefulProxy.Add(ip);
            //    }
            //});
            return listProxy;


        }

        public static bool IsProxyValid(string ipStr)
        {
            //构造Ping实例
            Ping pingSender = new Ping();
            //Ping 选项设置
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据
            string data = "test data abcabc";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //设置超时时间
            int timeout = 1000;
            //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
            PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                //Console.WriteLine("答复的主机地址：" + reply.Address.ToString());
                //Console.WriteLine("往返时间：" + reply.RoundtripTime);
                //Console.WriteLine("生存时间（TTL）：" + reply.Options.Ttl);
                //Console.WriteLine("是否控制数据包的分段：" + reply.Options.DontFragment);
                //Console.WriteLine("缓冲区大小：" + reply.Buffer.Length);
                return true;
            }
            else
                return false;
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

#if DEBUG

            Debug.WriteLine(url);
#endif
            System.Net.HttpWebRequest request = WebRequest.Create(url) as System.Net.HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>
        /// 获取HTML
        /// </summary>
        /// <param name="MyResponse"></param>
        /// <param name="encoding">字符编码</param>
        /// <param name="bufflen">数据包大小</param>
        /// <returns></returns>
        public static string GetHTML(HttpWebResponse MyResponse, Encoding encoding, int bufflen)
        {
            string strReturn = string.Empty;
            Stream stream = MyResponse.GetResponseStream();
            // 如果要下载的页面经过压缩，则先解压
            if (MyResponse.ContentEncoding.ToLower().IndexOf("gzip") >= 0)
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            strReturn += new StreamReader(stream, encoding).ReadToEnd();//解决乱码：utf-8 + streamreader.readtoend
            return strReturn;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            System.Net.HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as System.Net.HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as System.Net.HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
