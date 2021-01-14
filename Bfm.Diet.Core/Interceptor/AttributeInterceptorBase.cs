using System;
using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = true)]
    public abstract class AttributeInterceptorBase : Attribute, IInterceptor
    {
        public int Priority { get; set; }

        public virtual void Intercept(IInvocation invocation)
        {
        }
    }
}