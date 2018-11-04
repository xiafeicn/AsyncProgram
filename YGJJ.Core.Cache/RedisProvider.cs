using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using Newtonsoft.Json;
using ServiceStack.Redis;
using ServiceStack.Redis.Support;
using ServiceStack.Text;
using System.Linq;
using System.Web;
using StackExchange.Exceptional;

namespace YGJJ.Core.Cache
{
    public class RedisProvider : CacheProvider
    {
        public IEnumerable<string> ReadWriteHosts { get; set; }
        public IEnumerable<string> ReadOnlyHosts { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (string.IsNullOrEmpty(name))
                name = "CacheProvider";

            if (null == config)
                throw new ArgumentException("config参数不能为null");

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Redis.ReadWriteHosts"]) && string.IsNullOrEmpty(ConfigurationManager.AppSettings["Redis.ReadOnlyHosts"]))
                throw new ArgumentException("缺少Redis服务器配置");

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Redis Cache");
            }

            base.Initialize(name, config);

            string rwHosts = ConfigurationManager.AppSettings["Redis.ReadWriteHosts"] ?? "";
            string rHosts = ConfigurationManager.AppSettings["Redis.ReadOnlyHosts"] ?? "";

            ReadWriteHosts = rwHosts.Split(new[] { ';', ',', '|' });
            ReadOnlyHosts = rHosts.Split(new[] { ';', ',', '|' });
        }

        private static PooledRedisClientManager _prcm;
        public IRedisClient GetRedisClient()
        {
            _prcm = new PooledRedisClientManager(this.ReadWriteHosts, this.ReadOnlyHosts, new RedisClientManagerConfig()
            {
                AutoStart = true,
                MaxReadPoolSize = 5000,
                MaxWritePoolSize = 5000
            });
            return _prcm.GetClient();
        }

        public override bool ContainsKey(string key)
        {
            try
            {
                using (IRedisClient redis = GetRedisClient())
                {
                    return redis.ContainsKey(key);
                }
            }
            catch (Exception e)
            {
                ErrorStore.LogException(e, HttpContext.Current, applicationName: "redis");
            }
            return false;
        }

        public override bool Set<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = GetRedisClient())
                {
                    return redis.Set(key.ToString(), t);
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public override bool Set<T>(string key, T t, DateTime expire)
        {
            try
            {
                using (IRedisClient redis = GetRedisClient())
                {
                    return redis.Set(key.ToString(), t, expire);
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public override bool Set<T>(string key, T t, TimeSpan expireIn)
        {
            try
            {
                using (IRedisClient redis = GetRedisClient())
                {
                    return redis.Set(key.ToString(), t, expireIn);
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public override T Get<T>(string key)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                return redis.Get<T>(key.ToString());
            }
        }

        public override IDictionary<string, T> GetAll<T>(string[] key)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                return redis.GetAll<T>(key);
            }
        }

        public override bool Remove(string key)
        {
            try
            {
                using (IRedisClient redis = GetRedisClient())
                {
                    return redis.Remove(key.ToString());
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public override void RemoveAll(string[] key)
        {
            try
            {
                using (IRedisClient redis = GetRedisClient())
                {
                    redis.RemoveAll(key);
                }
            }
            catch (Exception e)
            {

            }
        }

        public override List<string> SearchKeys(string key)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                return redis.SearchKeys("*a");
            }
        }


        public override List<T> GetAllItemsFromList<T>(string listId)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                List<T> lstResult = new List<T>();
                var lstStr = redis.GetAllItemsFromList(listId);
                if (lstStr == null)
                {
                    return null;
                }
                foreach (var value in lstStr)
                {
                    lstResult.Add(JsonConvert.DeserializeObject<T>(value));
                }
                return lstResult;
            }
        }

        public override long GetListCount(string listId)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                return redis.GetListCount(listId);
            }
        }

        /// <summary>
        /// value +1
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override long IncrementValue(string key, int count = 1)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                return redis.IncrementValueBy(key, count);
            }
        }


        /**
         * 尝试获取分布式锁
         * @param jedis Redis客户端
         * @param lockKey 锁
         * @param requestId 请求标识
         * @param expireTime 超期时间
         * @return 是否获取成功
         */
        public override bool TryGetDistributedLock(String lockKey, String requestId, int expireTime)
        {
            long RELEASE_SUCCESS = 1L;
            using (IRedisClient redis = GetRedisClient())
            {
                return redis.SetEntryIfNotExists(lockKey, requestId);
            }
        }
        /**
         * 释放分布式锁
         * @param jedis Redis客户端
         * @param lockKey 锁
         * @param requestId 请求标识
         * @return 是否释放成功
         */
        public override bool ReleaseDistributedLock(String lockKey, String requestId)
        {
            long RELEASE_SUCCESS = 1L;
            using (IRedisClient redis = GetRedisClient())
            {
                String script = "if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";
                Object result = redis.ExecLuaAsInt(script, lockKey, requestId);

                if (RELEASE_SUCCESS.Equals(result))
                {
                    return true;
                }
                return false;
            }


        }

        /// <summary>
        /// value -1
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override long DecrementValue(string key, int count = 1)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                return redis.DecrementValueBy(key, count);
            }
        }

        public override T DequeueItemFromList<T>(string listId)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                var lstStr = redis.DequeueItemFromList(listId);
                if (lstStr == null)
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(lstStr);
            }
        }

        public override void LPush<T>(string listId, T item, DateTime? expireTime)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                var strValue = JsonConvert.SerializeObject(item, JsonSetting);
                redis.EnqueueItemOnList(listId, strValue);
                if (expireTime != null && expireTime.Value > DateTime.Now)
                {
                    redis.ExpireEntryAt(listId, expireTime.Value);
                }
            }
        }

        public override void RPush<T>(string listId, T value, DateTime? expireTime)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                var strValue = JsonConvert.SerializeObject(value, JsonSetting);
                redis.AddItemToList(listId, strValue);
                if (expireTime != null && expireTime.Value > DateTime.Now)
                {
                    redis.ExpireEntryAt(listId, expireTime.Value);
                }
            }
        }

        public override void RPushList<T>(string listId, List<T> values, DateTime? expireTime)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                List<string> lststr = new List<string>();
                foreach (var value in values)
                {
                    var strValue = JsonConvert.SerializeObject(value, JsonSetting);
                    lststr.Add(strValue);
                }
                if (lststr.Count > 0)
                {
                    redis.AddRangeToList(listId, lststr);
                }
                if (expireTime != null && expireTime.Value > DateTime.Now)
                {
                    redis.ExpireEntryAt(listId, expireTime.Value);
                }
            }
        }

        public override void TrimList(string listId, int keepStartingFrom, int keepEndingAt)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                redis.TrimList(listId, keepStartingFrom, keepEndingAt);
            }
        }

        public override void SetItemInList<T>(string listId, int listIndex, T value, DateTime? expireTime)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                redis.SetItemInList(listId, listIndex, JsonConvert.SerializeObject(value, JsonSetting));
                if (expireTime != null && expireTime.Value > DateTime.Now)
                {
                    redis.ExpireEntryAt(listId, expireTime.Value);
                }
            }
        }

        public override void DelItemInList(string listId, int listIndex)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                using (var trans = redis.CreateTransaction())
                {
                    redis.SetItemInList(listId, listIndex, "__deleted__");
                    redis.RemoveItemFromList(listId, "__deleted__", 1);
                    trans.Commit();
                }
            }
        }

        public override void RemoveByPattern(string pattern)
        {
            using (IRedisClient redis = GetRedisClient())
            {
                var keys = redis.SearchKeys(pattern: "*" + pattern + "*");
                foreach (var key in keys)
                    redis.Remove(key);
            }
        }

        public static JsonSerializerSettings JsonSetting
        {
            get
            {
                var setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                return setting;
            }
        }
    }
}
