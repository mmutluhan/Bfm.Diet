using System;
using System.Diagnostics;
using Bfm.Diet.Core.Cache.Redis;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Json;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Bfm.Diet.Core.Extensions;
using System.Linq.Expressions;
//using Bfm.Diet.Core.Cache.Memory;

namespace Bfm.Diet.Core.Interceptor.Attributes
{
    public class CacheResults : AttributeInterceptor
    {
        private RedisCache _cacheManager;
        //public MemoryCache CacheManager
        //{
        //    get
        //    {
        //        return _cacheManager ??= (MemoryCache) ServiceResolver.ServiceProvider.GetService(typeof(MemoryCache));
        //    }
        //}

        public RedisCache CacheManager
        {
            get
            {
                return _cacheManager ??= (RedisCache) ServiceResolver.ServiceProvider.GetService(typeof(RedisCache));
            }
        }
        /// <summary>
        ///  Lifetime Seconds
        /// </summary>
        public int Lifetime { get; set; }

        protected override void OnBefore(IInvocation invocation)
        {
            var key = CalculateCacheKey(invocation);
            var result = CacheManager.GetOrDefault(key);
            if (result != null)
                invocation.ReturnValue = result;
            Debug.WriteLine("Cache OnBefore ");
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            var key = CalculateCacheKey(invocation);
            CacheManager.Set(key, invocation.ReturnValue, TimeSpan.FromHours(1), TimeSpan.FromSeconds(Lifetime));
        }

        private string CalculateCacheKey(IInvocation invocation)
        {
            if (invocation.Arguments.Length > 0)
            {
                var expression = invocation.Arguments[0].UnboxExpression<object>();
                var args = expression.ResolveArgs();
            }
            return string.Concat(invocation.TargetType.FullName, ".", invocation.Method.Name, "(",
                JsonConvert.SerializeObject(invocation.Arguments, SerializerSettings.BfmJsonSerializerSettings), ")");
        }
    }
}