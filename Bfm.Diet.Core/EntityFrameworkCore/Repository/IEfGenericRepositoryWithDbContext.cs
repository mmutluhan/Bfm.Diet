using Microsoft.EntityFrameworkCore;

namespace Bfm.Diet.Core.EntityFrameworkCore.Repository
{
    public interface IEfGenericRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}