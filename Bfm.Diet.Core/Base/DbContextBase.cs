using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bfm.Diet.Core.EntityFrameworkCore.Audit;
using Bfm.Diet.Core.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bfm.Diet.Core.Base
{
    public abstract class DbContextBase : DbContext
    {
        private readonly IBfmSessionInfo _sessionInfo;

        protected DbContextBase(DbContextOptions options) : base(options)
        {
        }

        protected DbContextBase(DbContextOptions options, IBfmSessionInfo sessionInfo)
            : base(options)
        {
            _sessionInfo = sessionInfo;
            InitializeDbContext();
        }

        private void InitializeDbContext()
        {

        } 

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().ToList()) ApplyBfmConcepts(entry, _sessionInfo.Id);

            return base.SaveChanges();
        }

        private void ApplyBfmConcepts(EntityEntry entry, int? userId)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCreationAudit(entry, userId);
                    break;
                case EntityState.Modified:
                    SetUpdateAudit(entry, userId);
                    break;
                case EntityState.Deleted:
                    SetSoftDeletionAudit(entry);
                    break;
            }
        }

        protected virtual void SetCreationAudit(EntityEntry entityAsObj, int? userId)
        {
            EntityAuditingHelper.SetCreationAudit(entityAsObj, userId);
        }

        protected virtual void SetUpdateAudit(EntityEntry entityAsObj, int? userId)
        {
            EntityAuditingHelper.SetUpdateAudit(entityAsObj, userId);
        }

        protected virtual void SetSoftDeletionAudit(EntityEntry entityAsObj)
        {
            EntityAuditingHelper.SetSoftDeletionAudit(entityAsObj);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().ToList()) ApplyBfmConcepts(entry, _sessionInfo.Id);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}