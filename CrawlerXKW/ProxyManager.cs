using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Grabzujuan;
using Newtonsoft.Json.Linq;

namespace CrawlerXKW
{
    public static class ProxyManager
    {
        public static string proxy = "110.86.174.49:20677";

        public static bool HasExpire = false;

        public static string GetProxy()
        {
            if (HasExpire)
                return GetFromApi();
            return proxy;
        }

        public static string GetFromApi()
        {
            var res = HttpClientHolder.GetRequest("http://piping.mogumiao.com/proxy/api/get_ip_bs?appKey=70a2d5c92bf84a3e9c32fa95f9ca6abf&count=1&expiryDate=0&format=1&newLine=2");
            var p = (JObject.Parse(res)["msg"] as JArray)[0];
            return $"{p["ip"].NullToString()}:{p["port"].NullToString()}";
        }
    }
}
