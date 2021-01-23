using System;
using System.Threading.Tasks;
using Bfm.Diet.Core.Cache.Base;
using Bfm.Diet.Core.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ApplicationException = Bfm.Diet.Core.Exceptions.ApplicationException;

namespace Bfm.Diet.Core.Cache.Memory
{
    public class MemoryCache : CacheBase
    {
        private Microsoft.Extensions.Caching.Memory.MemoryCache _memoryCache;

        public MemoryCache(string name)
            : base(name)
        {
            _memoryCache =
                new Microsoft.Extensions.Caching.Memory.MemoryCache(
                    new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
        }

        public override object GetOrDefault(string key)
        {
            return _memoryCache.Get(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            if (value == null) throw new ApplicationException("Can not insert null values to the cache!");

            if (absoluteExpireTime != null)
                _memoryCache.Set(key, value, DateTimeOffset.Now.Add(absoluteExpireTime.Value));
            else if (slidingExpireTime != null)
                _memoryCache.Set(key, value, slidingExpireTime.Value);
            else if (DefaultAbsoluteExpireTime != null)
                _memoryCache.Set(key, value, DateTimeOffset.Now.Add(DefaultAbsoluteExpireTime.Value));
            else
                _memoryCache.Set(key, value, DefaultSlidingExpireTime);
        }

        public override void Set<T>(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            if (value == null) throw new ApplicationException("Can not insert null values to the cache!");

            if (absoluteExpireTime != null)
                _memoryCache.Set(key, value, DateTimeOffset.Now.Add(absoluteExpireTime.Value));
            else if (slidingExpireTime != null)
                _memoryCache.Set(key, value, slidingExpireTime.Value);
            else if (DefaultAbsoluteExpireTime != null)
                _memoryCache.Set(key, value, DateTimeOffset.Now.Add(DefaultAbsoluteExpireTime.Value));
            else
                _memoryCache.Set(key, value, DefaultSlidingExpireTime);
        }

        public override T Get<T>(string key, bool remove)
        {
            var item = GetOrDefault(key);
            if (item == null)
                return default;

            if (remove)
                Remove(key);

            return JsonConvert.DeserializeObject<T>(item.ToString(), SerializerSettings.BfmJsonSerializerSettings);
        }

        public override T GetOrAdd<T>(string key, Func<T> operation, int lifetime, bool refresh)
        {
            var item = Get<T>(key, false);
            if (item == null || refresh)
            {
                item = operation.Invoke();
                var cacheItem = JsonConvert.SerializeObject(item, SerializerSettings.BfmJsonSerializerSettings);
                Set(key, cacheItem, TimeSpan.FromSeconds(lifetime));
                return item;
            }

            return item;
        }

        public override async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> operation, int lifetime, bool refresh)
        {
            var item = Get<T>(key, false);
            if (item == null || refresh)
            {
                item = await operation.Invoke();
                var cacheItem = JsonConvert.SerializeObject(item, SerializerSettings.BfmJsonSerializerSettings);
                Set(key, cacheItem, TimeSpan.FromSeconds(lifetime));
                return await Task.FromResult(item).ConfigureAwait(false);
            }

            return await Task.FromResult(item).ConfigureAwait(false);
        }


        public override async Task<T> GetAsync<T>(string key, bool remove)
        {
            try
            {
                var item = Get<T>(key, remove);
                if (item == null)
                    return await Task.FromResult(default(T)).ConfigureAwait(false);

                if (remove)
                    Remove(key);

                var obj = JsonConvert.DeserializeObject<T>(item.ToString(),
                    SerializerSettings.BfmJsonSerializerSettings);
                return await Task.FromResult(obj).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on retrieving cache :" + ex.Message);
            }
        }

        public override void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public override void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache =
                new Microsoft.Extensions.Caching.Memory.MemoryCache(
                    new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
        }

        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }
    }
}