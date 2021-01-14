using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    public static class ReflectionExtension
    {
        public static bool IsOverride(this MethodInfo methodInfo)
        {
            if ((methodInfo.Attributes & MethodAttributes.NewSlot) == MethodAttributes.NewSlot)
                return true;
            return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
        }

        public static MethodType GeMethodType(this IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        public static IEnumerable<TypedParameter> GetTypedResolvingParameters(this object @this)
        {
            foreach (var propertyInfo in @this.GetType().GetTypeInfo().GetProperties())
                yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(@this, null));
        }
    }
}