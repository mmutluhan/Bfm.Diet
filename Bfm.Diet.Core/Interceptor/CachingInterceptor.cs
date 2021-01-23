using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bfm.Diet.Core.Attributes;
using Bfm.Diet.Core.Cache.Redis;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Exceptions;
using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    public class CachingInterceptor : InterceptorBase<CacheAttribute>
    {
        private static readonly HashSet<Type> AvailableKeyTypes = new HashSet<Type>
            {typeof(decimal), typeof(decimal?), typeof(string), typeof(DateTime), typeof(DateTime?)};

        private RedisCache _cacheManager;


        public CachingInterceptor(IExceptionHandler handler) : base(handler)
        {
        }

        public RedisCache CacheManager
        {
            get
            {
                return _cacheManager ??= (RedisCache) ServiceResolver.ServiceProvider.GetService(typeof(RedisCache));
            }
        }

        protected override void InterceptInner(IInvocation invocation)
        {
            invocation.Proceed();
        }

        protected override T InterceptInnerWithResult<T>(IInvocation invocation)
        {
            var key = BuildCacheKeyFrom(invocation);
            return InterceptWithResult(invocation, lifeTime => CacheManager.GetOrAdd(key, () =>
            {
                invocation.Proceed();
                return (T) invocation.ReturnValue;
            }, lifeTime, false));
        }

        private T InterceptWithResult<T>(IInvocation invocation, Func<int, T> setToCacheFunc)
        {
            var attribute = HasCacheAttribute(invocation);
            if (attribute != null && CanCache(attribute, invocation))
            {
                var result = setToCacheFunc(attribute.Lifetime);
                if (result != null)
                    return result;
            }

            invocation.Proceed();
            return (T) invocation.ReturnValue;
        }

        protected override async Task InterceptInnerAsync(IInvocation invocation)
        {
            invocation.Proceed();
            await (Task) invocation.ReturnValue;
        }

        protected override Task<T> InterceptInnerWithResultAsync<T>(IInvocation invocation)
        {
            var key = BuildCacheKeyFrom(invocation);
            return InterceptWithResult(invocation, lifeTime => CacheManager.GetOrAddAsync(key, async () =>
                {
                    invocation.CaptureProceedInfo().Invoke();
                    var taskResult = (Task<T>) invocation.ReturnValue;
                    var methodResult = await taskResult;
                    return methodResult;
                },
                lifeTime,
                false));
        }


        private bool CanCache(CacheAttribute cacheAttribute, IInvocation invocation)
        {
            if (cacheAttribute.Lifetime <= 1)
                throw new ArgumentException(
                    $"Invalid cache lifetime ({cacheAttribute.Lifetime}) for {invocation.TargetType}, method {invocation.Method.Name}");
            return cacheAttribute.Enabled;
        }

        private static string BuildCacheKeyFrom(IInvocation invocation)
        {
            var argsList = new List<string>();
            if (invocation.GenericArguments != null) argsList.AddRange(invocation.GenericArguments.Select(g => g.Name));

            foreach (var argument in invocation.Arguments)
            {
                var type = argument?.GetType();

                if (type == null)
                    argsList.Add("null");
                else if (AvailableKeyTypes.Contains(type) || type.IsPrimitive || type.IsEnum)
                    argsList.Add(Convert.ToString(argument, CultureInfo.InvariantCulture));
                else
                    throw new ArgumentException(
                        $"All arguments of method {invocation.Method.Name} of class {invocation.TargetType.Name}");
            }

            var methodName = $"{invocation.TargetType.Name}.{invocation.Method.Name.Replace("Async", "")}";
            var argsString = string.Join(" + ", argsList);

            var cacheKey = $"<{methodName}>:<{argsString}>";
            return cacheKey;
        }
    }
}