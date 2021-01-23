using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bfm.Diet.Core.Attributes;
using Bfm.Diet.Core.Exceptions;
using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    public abstract class InterceptorBase<TAttribute> : IInterceptor where TAttribute : BaseAttribute
    {
        private static readonly MethodInfo HandleMethodInfo = typeof(InterceptorBase<TAttribute>).GetMethod(
            nameof(InterceptInnerWithResult),
            BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo HandleAsyncMethodInfo = typeof(InterceptorBase<TAttribute>).GetMethod(
            nameof(InterceptInnerWithResultAsync),
            BindingFlags.Instance | BindingFlags.NonPublic);

        protected static readonly List<Func<IInvocation, TAttribute>> AttributeSelectors =
            new List<Func<IInvocation, TAttribute>>
            {
                invocation => invocation.MethodInvocationTarget.GetCustomAttribute<TAttribute>(),
                invocation => invocation.TargetType.GetCustomAttribute<TAttribute>(),
                invocation => invocation.Method.GetCustomAttribute<TAttribute>(),
                invocation => invocation.Method.DeclaringType?.GetCustomAttribute<TAttribute>()
            };

        private readonly IExceptionHandler _handler;

        protected InterceptorBase(IExceptionHandler handler)
        {
            _handler = handler;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!CanIntercept(invocation))
            {
                invocation.Proceed();
                return;
            }

            var delegateType = GetDelegateType(invocation);
            switch (delegateType)
            {
                case MethodType.Void:
                    InterceptInner(invocation);
                    break;
                case MethodType.Synchronous:
                    HandleExceptions(() =>
                        ExecuteHandleWithResultUsingReflection(HandleMethodInfo, invocation.Method.ReturnType,
                            invocation));
                    break;
                case MethodType.AsyncAction:
                    HandleExceptions(() => InterceptInnerAsync(invocation));
                    break;
                case MethodType.AsyncFunction:
                    var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
                    HandleExceptions(() =>
                        ExecuteHandleWithResultUsingReflection(HandleAsyncMethodInfo, resultType, invocation));
                    break;
            }
        }

        protected abstract void InterceptInner(IInvocation invocation);
        protected abstract T InterceptInnerWithResult<T>(IInvocation invocation);
        protected abstract Task InterceptInnerAsync(IInvocation invocation);
        protected abstract Task<T> InterceptInnerWithResultAsync<T>(IInvocation invocation);

        protected static TAttribute HasCacheAttribute(IInvocation invocation)
        {
            return AttributeSelectors.Select(selector => selector(invocation))
                .FirstOrDefault(attribute => attribute != null);
        }

        private void ExecuteHandleWithResultUsingReflection(MethodInfo handleMethodInfo, Type resultType,
            IInvocation invocation)
        {
            var mi = handleMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new object[] {invocation});
        }

        public void HandleExceptions(Action act)
        {
            _handler.HandleExceptions(act);
        }

        private static bool CanIntercept(IInvocation invocation)
        {
            var attribute = HasCacheAttribute(invocation);
            return attribute != null && attribute.Enabled;
        }

        private static MethodType GetDelegateType(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(void))
                return MethodType.Void;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }
    }
}