using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;

namespace YGJJ.Core.Cache
{
    public class NullCacheProvider : CacheProvider
    {
        public override bool ContainsKey(string key)
        {
            return false;
        }

        public override bool Set<T>(string key, T t)
        {
            return false;
        }

        public override bool Set<T>(string key, T t, DateTime expire)
        {
            return false;
        }

        public override bool Set<T>(string key, T t, TimeSpan expireIn)
        {
            return false;
        }

        public override T Get<T>(string key)
        {
            return default(T);
        }

        public override bool Remove(string key)
        {
            return true;
        }

        public override void RemoveAll(string[] key)
        {
        }

        public override List<string> SearchKeys(string key)
        {
            return null;
        }

        public override List<T> GetAllItemsFromList<T>(string listId)
        {
            return null;
        }

        public override T DequeueItemFromList<T>(string listId)
        {
            return default(T);
        }

        public override long GetListCount(string listId)
        {
            return default(long);
        }

        public override long IncrementValue(string key, int count = 1)
        {
            return default(long);
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();

        }
        public override long DecrementValue(string key, int count = 1)
        {
            return default(long);
        }


        public override void LPush<T>(string listId, T item, DateTime? expireTime)
        {
        }

        public override void RPush<T>(string listId, T value, DateTime? expireTime)
        {
        }

        public override void RPushList<T>(string listId, List<T> values, DateTime? expireTime)
        {
        }

        public override void TrimList(string listId, int keepStartingFrom, int keepEndingAt)
        {
        }

        public override void SetItemInList<T>(string listId, int listIndex, T value, DateTime? expireTime)
        {
        }

        public override void DelItemInList(string listId, int listIndex)
        {
        }
        public override void RemoveByPattern(string pattern)
        {
        }

        public override IDictionary<string, T> GetAll<T>(string[] key)
        {
            return null;
        }
    }
}
