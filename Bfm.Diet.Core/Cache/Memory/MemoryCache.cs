using System;
using Bfm.Diet.Core.Cache.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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