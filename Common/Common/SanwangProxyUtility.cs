﻿using System;
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
    public class SanwangProxyUtility
    {

        private static object objlock = new object();

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public SanwangProxyUtility()
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

                    fillProxy(request);

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

                  
#if DEBUG
                    stopWatch.Stop();
                    Debug.WriteLine($"cost {stopWatch.ElapsedMilliseconds} ms,url{url}");
                    Console.WriteLine($"cost {stopWatch.ElapsedMilliseconds} ms,url{url}");
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
        private static void fillProxy(HttpWebRequest request)
        {

            //IP: 帐号: tets1106密码: tets1106开通成功！http端口：808
            //创建 代理服务器设置对象 的实例
            System.Net.WebProxy wp = new System.Net.WebProxy("123.249.2.2:888");
            //代理服务器需要验证
            wp.BypassProxyOnLocal = false;
            //用户名密码
            wp.Credentials = new NetworkCredential("te1107", "te1107");

            request.Proxy = wp;
        }
    }
}
