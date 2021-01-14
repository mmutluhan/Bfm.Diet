using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    public class CustomInterceptionSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            IEnumerable<AttributeInterceptorBase> methodAttributes;
            var classAttributes = type.GetCustomAttributes<AttributeInterceptorBase>(true).ToList();
            if (!method.IsOverride())
                methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<AttributeInterceptorBase>(true);
            else
                methodAttributes = type.GetMethods().FirstOrDefault(x => x.Name == method.Name && x.IsOverride())
                    .GetCustomAttributes<AttributeInterceptorBase>(true);

            classAttributes.AddRange(methodAttributes);
            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}