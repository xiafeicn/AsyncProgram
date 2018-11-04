using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace YGJJ.Core.Cache
{
    public abstract class CacheProvider : ProviderBase
    {
        public abstract bool ContainsKey(string key);

        public abstract bool Set<T>(string key, T t);

        public abstract bool Set<T>(string key, T t, DateTime expire);

        public abstract bool Set<T>(string key, T t, TimeSpan expireIn);

        public abstract T Get<T>(string key);
        public abstract IDictionary<string,T> GetAll<T>(string[] key);
        public abstract bool Remove(string key);

        public abstract void RemoveAll(string[] key);
        public abstract List<string> SearchKeys(string key);

        public abstract List<T> GetAllItemsFromList<T>(string listId);
        public abstract T DequeueItemFromList<T>(string listId) where T : class ;
        public abstract long GetListCount(string listId);

        public abstract long IncrementValue(string key, int count = 1);

        public abstract long DecrementValue(string key, int count = 1);

        public abstract void LPush<T>(string listId, T item, DateTime? expireTime);

        public abstract void RPush<T>(string listId, T value, DateTime? expireTime);

        public abstract void RPushList<T>(string listId, List<T> values, DateTime? expireTime);

        public abstract void TrimList(string listId, int keepStartingFrom, int keepEndingAt);

        public abstract void SetItemInList<T>(string listId, int listIndex, T value, DateTime? expireTime);

        public abstract void DelItemInList(string listId, int listIndex);

        public abstract void RemoveByPattern(string pattern);

        public abstract bool TryGetDistributedLock(String lockKey, String requestId, int expireTime);
        public abstract bool ReleaseDistributedLock(String lockKey, String requestId);

    }
}
