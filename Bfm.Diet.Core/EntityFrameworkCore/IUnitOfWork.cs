using System.Threading.Tasks;
using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bfm.Diet.Core.EntityFrameworkCore
{
    public interface IUnitOfWork<TContext> where TContext : DbContextBase
    {
        TContext Context { get; }
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        void CommitTransaction(IDbContextTransaction transaction);
        void Rollback();
        Task RollbackAsync();
        void Commit();
        Task CommitAsync();
    }
}