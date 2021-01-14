namespace Bfm.Diet.Core.Extensions
{
    public static class ObjectExtension
    {
        public static object GetPropValueFromObject(this object src, string propName)
        {
            if (src == null)
                return null;

            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}