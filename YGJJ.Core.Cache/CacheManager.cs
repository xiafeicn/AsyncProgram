using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace YGJJ.Core.Cache
{
    public class CacheManager
    {
        #region private methods
        private static bool _mIsInitialized = false;

        /// <summary>
        /// 静态构造函数，初始化
        /// </summary>
        static CacheManager()
        {
            Providers = null;
            Provider = null;
            Initialize();
        }

        private static void Initialize()
        {
            try
            {
                if (!_mIsInitialized)
                {

                        // 找到配置文件中“CacheProvider”节点
                        var cacheConfig = (CacheProviderSection)ConfigurationManager.GetSection("CacheProvider");

                    if (cacheConfig == null)
                        throw new ConfigurationErrorsException("在配置文件中没找到“CacheProvider”节点");



                    Providers = new CacheProviderCollection();

                    if (ConfigurationManager.AppSettings["cacheenable"] == "true")
                    {
                        Provider = new NullCacheProvider();
                        _mIsInitialized = true;
                        return;
                    }
                    // 使用System.Web.Configuration.ProvidersHelper类调用每个Provider的Initialize()方法
                    ProvidersHelper.InstantiateProviders(cacheConfig.Providers, Providers, typeof(CacheProvider));
                    // 所用的Provider为配置中默认的Provider
                    Provider = Providers[cacheConfig.DefaultProvider] as CacheProvider;
                    _mIsInitialized = true;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception(msg);
            }
        }

        private static CacheProvider Provider { get; set; }

        private static CacheProviderCollection Providers { get; set; }
        #endregion

        #region public methods

        public static bool ContainsKey(string key)
        {
            return Provider.ContainsKey(key);
        }

        public static bool Set<T>(string key, T t)
        {
            return Provider.Set<T>(key, t);
        }

        public static bool Set<T>(string key, T t, DateTime? expire)
        {
            return expire.HasValue ? Provider.Set<T>(key, t, expire.Value) : Provider.Set<T>(key, t);
        }

        public static bool Set<T>(string key, T t, DateTime expire)
        {
            return Provider.Set<T>(key, t, expire);
        }

        public static bool Set<T>(string key, T t, int seconds)
        {
            return Provider.Set<T>(key, t, DateTime.Now.AddSeconds(seconds));
        }

        public static T Get<T>(string key,DateTime dateTime, Func<T> acquire)
            where T : class 
        {
            var model = Get<T>(key);
            if (model == null)
            {
                model = acquire();
                if (model == null) return null;
                Set(key, model, dateTime);
            }
            return model;
        }

        public static bool Insert<T>(string key, T t, int seconds)
        {
            return Set(key, t, DateTime.Now.AddSeconds(seconds));
        }

        public static bool Insert<T>(string key, T t)
        {
            try
            {
                return Set(key, t);
            }
            catch (Exception)
            {

            }
            return false;
        }

        public static bool Set<T>(string key, T t, TimeSpan expireIn)
        {
            try
            {
                return Provider.Set<T>(key, t, expireIn);

            }
            catch (Exception)
            {

            }
            return false;
        }

        public static T Get<T>(string key)
        {
            try
            {
                return Provider.Get<T>(key);
            }
            catch
            {


            }
            return default(T);
        }

        public static IDictionary<string,T> GetAll<T>(string[] keys)
        {
            try
            {
                return Provider.GetAll<T>(keys);
            }
            catch
            {
            }
            return new Dictionary<string, T>();
        }


        public static bool Remove(string key)
        {
            return Provider.Remove(key);
        }
        public static void RemoveAll(string[] keys)
        {
            Provider.RemoveAll(keys);
        }



        public static List<T> GetAllItemsFromList<T>(string listId)
        {
            return Provider.GetAllItemsFromList<T>(listId);
        }

        public static T DequeueItemFromList<T>(string listId) where T : class 
        {
            return Provider.DequeueItemFromList<T>(listId);
        }

        public static long GetListCount(string listId)
        {
            return Provider.GetListCount(listId);
        }

        public static bool TryGetDistributedLock(String lockKey, String requestId, int expireTime)
        {
            return Provider.TryGetDistributedLock(lockKey, requestId, expireTime);
        }
        /**
         * 释放分布式锁
         * @param jedis Redis客户端
         * @param lockKey 锁
         * @param requestId 请求标识
         * @return 是否释放成功
         */
        public static bool ReleaseDistributedLock(String lockKey, String requestId)
        {
            return Provider.ReleaseDistributedLock(lockKey, requestId);

        }
        public static long IncrementValue(string key, int count = 1)
        {
            return Provider.IncrementValue(key);
        }

        public static long DecrementValue(string key, int count = 1)
        {
            return Provider.DecrementValue(key);
        }
        public static void LPush<T>(string listId, T value, DateTime? expireTime = null)
        {
            Provider.LPush(listId, value, expireTime);
        }

        public static void RPush<T>(string listId, T value, DateTime? expireTime = null)
        {
            Provider.RPush(listId, value, expireTime);
        }

        public static void RPushList<T>(string listId, List<T> value, DateTime? expireTime = null)
        {
            Provider.RPushList(listId, value, expireTime);
        }

        public static void TrimList(string listId, int keepStartingFrom, int keepEndingAt)
        {
            Provider.TrimList(listId, keepStartingFrom, keepEndingAt);
        }

        public static void SetItemInList<T>(string listId, int listIndex, T value, DateTime? expireTime = null)
        {
            Provider.SetItemInList(listId, listIndex, value, expireTime);

        }

        public static void DelItemInList(string listId, int listIndex)
        {
            Provider.DelItemInList(listId, listIndex);
        }
        public static void RemoveByPattern(string pattern)
        {
            Provider.RemoveByPattern(pattern);
        }

        #endregion
    }
}
