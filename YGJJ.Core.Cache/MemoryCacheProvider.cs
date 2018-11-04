using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ServiceStack.Redis;
using StackExchange.Exceptional;

namespace YGJJ.Core.Cache
{
    public class MemoryCacheProvider
    {
        protected MemoryCache _cache;

        public MemoryCacheProvider()
        {
            _cache = MemoryCache.Default;
        }
        public bool ContainsKey(string key)
        {
            try
            {
                return _cache.Contains(key);
            }
            catch (Exception e)
            {
                ErrorStore.LogException(e, HttpContext.Current, applicationName: "memory");
            }
            return false;
        }

        public bool Set<T>(string key, T t)
        {
            try
            {
                _cache.Set(key, t, new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable });
            }
            catch (Exception e)
            {
                ErrorStore.LogException(e, HttpContext.Current, applicationName: "memory");
            }
            return false;
        }

        public bool Set<T>(string key, T t, DateTime expire)
        {
            try
            {
                _cache.Set(key, t, new CacheItemPolicy() { AbsoluteExpiration = expire });
            }
            catch (Exception e)
            {
                ErrorStore.LogException(e, HttpContext.Current, applicationName: "memory");

            }
            return false;
        }


        public  bool Set<T>(string key, T t, int seconds)
        {
            try
            {
                _cache.Set(key, t, new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(seconds)});
            }
            catch (Exception e)
            {
                ErrorStore.LogException(e, HttpContext.Current, applicationName: "memory");

            }
            return false;
        }


        public T Get<T>(string key)
        {
            return (T)_cache.Get(key.ToString(), null);
        }
    }
}
