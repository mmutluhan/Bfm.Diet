using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    public static class ReflectionExtension
    {
        public static MethodType GeMethodType(this IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        public static Func<object> BuildFuncAccessor(this MethodInfo method)
        {
            var obj = Expression.Parameter(typeof(object), "o");

            var expr =
                Expression.Lambda<Func<object>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(obj, method.DeclaringType),
                            method),
                        typeof(object)),
                    obj);

            return expr.Compile();
        }

        public static bool IsAttributeExists<T>(this MethodInfo method) where T : Attribute
        {
            return method.GetCustomAttribute<T>(false) != null;
        }

        public static Dictionary<string, string> GetMethodParameters(this MethodInfo method)
        {
            var dic = method.GetParameters().Select(x => new {x.Name, ParameterType = x.ParameterType.Name})
                .ToDictionary(k => k.Name, k => k.ParameterType);
            return dic;
        }

        public static IEnumerable<TypedParameter> GetTypedResolvingParameters(this object @this)
        {
            foreach (var propertyInfo in @this.GetType().GetTypeInfo().GetProperties())
                yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(@this, null));
        }
    }
}