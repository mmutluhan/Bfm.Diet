using System.Collections.Generic;
using System.Threading.Tasks;
using Bfm.Diet.Core.Mapper;

namespace Bfm.Diet.Core.Extensions
{
    public static class ConverterExtension
    {
        public static IEnumerable<TEntity> ToEntityList<TEntity, TDto>(this IEnumerable<TDto> obj)
        {
            return DietMapper.Mapper.Map<IEnumerable<TDto>, IEnumerable<TEntity>>(obj);
        }

        public static IEnumerable<TDto> ToDtoList<TDto, TEntity>(this IEnumerable<TEntity> obj)
        {
            return DietMapper.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(obj);
        }

        public static TEntity ToEntity<TEntity, TDto>(this TDto obj)
        {
            return DietMapper.Mapper.Map<TDto, TEntity>(obj);
        }

        public static TDto ToDto<TDto, TEntity>(this TEntity obj)
        {
            return DietMapper.Mapper.Map<TEntity, TDto>(obj);
        }


        public static Task<IEnumerable<TEntity>> ToEntityListAsync<TEntity, TDto>(this Task<IEnumerable<TDto>> obj)
        {
            return DietMapper.Mapper.MapAsync<IEnumerable<TDto>, IEnumerable<TEntity>>(obj);
        }

        public static Task<IEnumerable<TDto>> ToDtoListAsync<TDto, TEntity>(this Task<IEnumerable<TEntity>> obj)
        {
            return DietMapper.Mapper.MapAsync<IEnumerable<TEntity>, IEnumerable<TDto>>(obj);
        }

        public static Task<TEntity> ToEntityAsync<TEntity, TDto>(this Task<TDto> obj)
        {
            return DietMapper.Mapper.MapAsync<TDto, TEntity>(obj);
        }

        public static Task<TDto> ToDtoAsync<TDto, TEntity>(this Task<TEntity> obj)
        {
            return DietMapper.Mapper.MapAsync<TEntity, TDto>(obj);
        }
    }
}