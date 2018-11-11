using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Common.Common
{
    /// <summary>
    ///     Requests操作类
    /// </summary>
    public class HttpWebRequestUtility
    {
        /// <summary>
        ///     取IP
        /// </summary>
        /// <returns>返回IP</returns>
        public static string GetIP()
        {
            var result = HttpContext.Current.Request.Headers.Get("X-Forwarded-For");
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr");
            }
            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }
            
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (result != null && result != String.Empty)
            {
//可能有代理 
                if (result.IndexOf(".") == -1) result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
//有“,”，估计多个代理。取第一个不是内网的IP。 

                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (temparyip[i].IsIP()
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i]; //找到不是内网的地址 
                            }
                        }
                    }
                    else if (result.IsIP()) //代理即是IP格式 
                        return result;
                    else
                        result = null; //代理中的内容 非IP，取IP 
                }
            }

            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
                                && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty)
                                   ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                                   : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (null == result || result == String.Empty)
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;

            return result;
        }
       
        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[128];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static string GetOs()
        {
            var userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            var osVersion = "未知";
            if (userAgent.Contains("NT 6.2"))
            {
                osVersion = "Windows 8";
            }

            if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows 7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;  
        }

        public static object Post(string url, string postData)
        {
            byte[] bs = Encoding.ASCII.GetBytes(postData);
            var req = (System.Net.HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 90000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            req.AllowAutoRedirect = false;
            req.UserAgent =
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";


            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8)) 
            {
                return sr.ReadToEnd();
            }
        }
    }
}