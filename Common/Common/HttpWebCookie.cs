using System;
using System.Configuration;
using System.Web;

namespace Common.Common
{
    public class HttpWebCookie
    {
        public static readonly string DoMain = ConfigurationManager.AppSettings.Get("doMain");
        /// <summary>
        ///     Cookies赋值
        /// </summary>
        /// <param name="strName">主键</param>
        /// <param name="strValue">键值</param>
        /// <param name="strDay">有效天数</param>
        /// <returns></returns>
        public static bool SetCookie(string strName, string strValue, int strDay)
        {
            try
            {
                var Cookie = new HttpCookie(strName);
                Cookie.Domain =  DoMain;//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                Cookie.Expires = DateTime.Now.AddDays(strDay);
                Cookie.Value = strValue;
                HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     读取Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns></returns>
        public static string GetCookie(string strName)
        {
            HttpCookie Cookie = HttpContext.Current.Request.Cookies[strName];
            if (Cookie != null)
            {
                return Cookie.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     删除Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns></returns>
        public static bool DelCookie(string strName)
        {
            try
            {
                var Cookie = new HttpCookie(strName);
                Cookie.Domain = DoMain;//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                Cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}