using Microsoft.EntityFrameworkCore;

namespace Bfm.Diet.Core.Base
{
    public abstract class DbContextBase : DbContext
    {
        protected DbContextBase(DbContextOptions options)
            : base(options)
        {
            InitializeDbContext();
        }

        private void InitializeDbContext()
        {
        }
    }
}