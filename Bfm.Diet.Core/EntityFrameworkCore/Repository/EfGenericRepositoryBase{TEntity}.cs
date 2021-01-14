using Bfm.Diet.Core.Base;
using Bfm.Diet.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bfm.Diet.Core.EntityFrameworkCore.Repository
{
    public class EfGenericRepositoryBase<TDbContext, TEntity> : EfGenericRepositoryBase<TDbContext, TEntity, int>,
        IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public EfGenericRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}