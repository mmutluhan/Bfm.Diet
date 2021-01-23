using System;

namespace Bfm.Diet.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = true)]
    public class CacheAttribute : BaseAttribute
    {
        public int Lifetime { get; set; }
    }
}