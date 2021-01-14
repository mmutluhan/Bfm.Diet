using System.Collections.Generic;
using System.Reflection;
using Autofac;

namespace Bfm.Diet.Core.Dependency
{
    internal static class Extensions
    {
        internal static IEnumerable<TypedParameter> GetTypedResolvingParameters(this object @this)
        {
            foreach (var propertyInfo in @this.GetType().GetTypeInfo().GetProperties())
                yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(@this, null));
        }
    }
}