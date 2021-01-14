using Microsoft.EntityFrameworkCore;

namespace Bfm.Diet.Core.EntityFrameworkCore
{
    public sealed class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        public DbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext DbContext { get; }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }
    }
}