using Bfm.Diet.Core.Base;

namespace Bfm.Diet.Core.Repository
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }
}