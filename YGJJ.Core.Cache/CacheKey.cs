using System;
using System.Collections.Generic;
using System.Linq;

namespace YGJJ.Core.Cache
{
    public class CacheKey
    {
        public string CacheKeyFormat = "{0}://{1}?{2}";
        public string CacheDomain { get; set; }
        public string CacheDataType { get; set; }
        public string CacheUri { get; set; }

        public CacheKey() : this(Cache.CacheDomain.Default, Cache.CacheDataType.Data) { }

        public CacheKey(CacheDomain domain) : this(domain, Cache.CacheDataType.Data) { }

        public CacheKey(CacheDomain domain, CacheDataType type)
        {
            CacheDomain = domain.ToString();
            CacheDataType = type.ToString();
            Params = new List<CacheParam>();
        }

        public CacheKey(string domain, string type)
        {
            CacheDomain = domain;
            CacheDataType = type;
            Params = new List<CacheParam>();
        }

        public List<CacheParam> Params { get; set; }        

        public override string ToString()
        {
            Params.Sort();
            var s = Params.Aggregate(string.Empty, (current, param) => current + (string.Format("{0}={1}", param.Name, param.Value) + "&"));
            if (string.IsNullOrWhiteSpace(s))
                throw new Exception("cache key can not be empty!");
            s = s.Substring(0, s.Length - 1);
            s = string.Format(CacheKeyFormat, CacheDomain, CacheDataType, s);
            s = s.ToLower();
            return s;
        }
    }
}
