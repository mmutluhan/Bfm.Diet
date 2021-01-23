using System;

namespace Bfm.Diet.Core.Attributes
{
    public abstract class BaseAttribute : Attribute
    {
        public bool Enabled { get; set; } = true;
    }
}