using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YGJJ.Core.Cache
{
    public class CacheHelper 
    {
        public T Set<T>(string key, Func<T> func, int seconds = 86400)
        {
            var cache = CacheManager.Get<T>(key);
            if (null == cache)
            {
                T data = func();
                Insert(key, data, seconds);
                return data;
            }
            return cache;
        }

        public void Insert<T>(string key, T data, int seconds = 86400)
        {
            CacheManager.Insert(key, data, seconds);
        }

        public T Get<T>(string key)
        {
            return (T)CacheManager.Get<T>(key);
        }
    }
}
