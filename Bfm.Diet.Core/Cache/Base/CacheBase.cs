using System;
using System.Threading.Tasks;
using Bfm.Diet.Core.Utilities;

namespace Bfm.Diet.Core.Cache.Base
{
    public abstract class CacheBase : ICache
    {
        private readonly AsyncLock _asyncLock = new AsyncLock();

        protected readonly object SyncObj = new object();

        protected CacheBase(string name)
        {
            Name = name;
            DefaultSlidingExpireTime = TimeSpan.FromHours(1);
        }

        public string Name { get; }

        public TimeSpan DefaultSlidingExpireTime { get; set; }

        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        public virtual object Get(string key, Func<string, object> factory)
        {
            var cacheKey = key;
            var item = GetOrDefault(key);
            if (item == null)
                lock (SyncObj)
                {
                    item = GetOrDefault(key);
                    if (item == null)
                    {
                        item = factory(key);
                        if (item == null) return null;

                        Set(cacheKey, item);
                    }
                }

            return item;
        }

        public virtual async Task<object> GetAsync(string key, Func<string, Task<object>> factory)
        {
            var cacheKey = key;
            var item = await GetOrDefaultAsync(key);
            if (item == null)
                using (await _asyncLock.LockAsync())
                {
                    item = await GetOrDefaultAsync(key);
                    if (item == null)
                    {
                        item = await factory(key);
                        if (item == null) return null;

                        await SetAsync(cacheKey, item);
                    }
                }

            return item;
        }

        public abstract object GetOrDefault(string key);

        public virtual Task<object> GetOrDefaultAsync(string key)
        {
            return Task.FromResult(GetOrDefault(key));
        }

        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null);

        public virtual Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            Set(key, value, slidingExpireTime);
            return Task.FromResult(0);
        }


        public abstract void Remove(string key);

        public virtual Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.FromResult(0);
        }

        public abstract void Clear();

        public virtual Task ClearAsync()
        {
            Clear();
            return Task.FromResult(0);
        }

        public virtual void Dispose()
        {
        }

        public abstract T Get<T>(string key, bool remove);
        public abstract Task<T> GetAsync<T>(string key, bool remove);
        public abstract T GetOrAdd<T>(string key, Func<T> operation, int lifetime, bool refresh);
        public abstract Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> operation, int lifetime, bool refresh);

        public abstract void Set<T>(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null);

        public virtual Task SetAsync<T>(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            Set<T>(key, value, slidingExpireTime);
            return Task.FromResult(0);
        }
    }
}