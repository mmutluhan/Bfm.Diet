using System.Collections.Generic;
using Bfm.Diet.Core.Mapper;

namespace Bfm.Diet.Dto.Extensions
{
    public static class ObjectMappingExtensions
    {
        public static IEnumerable<TDest> Map<TSource, TDest>(this IEnumerable<TSource> obj)
        {
            return DietMapper.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDest>>(obj);
        }

        public static TDest Map<TSource, TDest>(this TSource obj)
        {
            return DietMapper.Mapper.Map<TSource, TDest>(obj);
        }
    }
}