using System;
using System.Data;
using System.Threading.Tasks;
using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Bfm.Diet.Core.EntityFrameworkCore
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContextBase
    {
        private readonly ILogger<UnitOfWork<TContext>> _logger;

        public UnitOfWork(TContext context, ILogger<UnitOfWork<TContext>> logger
        )
        {
            Context = context;
            _logger = logger;
        }

        public IDbContextTransaction CurrentTransaction { get; private set; }

        public bool HasActiveTransaction => CurrentTransaction != null;

        public TContext Context { get; private set; }

        public IDbContextTransaction BeginTransaction()
        {
            if (CurrentTransaction != null) return CurrentTransaction;


            CurrentTransaction = Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            _logger.LogError("currentTransaction KEY :" + CurrentTransaction.TransactionId);
            return CurrentTransaction;
        }

        public void Rollback()
        {
            CurrentTransaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            await CurrentTransaction.RollbackAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (CurrentTransaction != null) return CurrentTransaction;

            CurrentTransaction = await Context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return CurrentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            try
            {
                _logger.LogError("Commiting Transaction KEY :" + transaction.TransactionId);
                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Commiting Transaction KEY :" + transaction.TransactionId, ex);
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        public void CommitTransaction(IDbContextTransaction transaction)
        {
            try
            {
                Context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        public void Commit()
        {
            try
            {
                Context.SaveChanges();
                CurrentTransaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
                await CurrentTransaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    await CurrentTransaction.DisposeAsync();
                    CurrentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                _logger.LogError("Rollback Transaction KEY :" + CurrentTransaction.TransactionId);
                await CurrentTransaction?.RollbackAsync();
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    await CurrentTransaction.DisposeAsync();
                    CurrentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _logger.LogError("Rollback Transaction KEY :" + CurrentTransaction.TransactionId);
                CurrentTransaction?.Rollback();
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return CurrentTransaction;
        }


        public void Dispose()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }

            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }
    }
}