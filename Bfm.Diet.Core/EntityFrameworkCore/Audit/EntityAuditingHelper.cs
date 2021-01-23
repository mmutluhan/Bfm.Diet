using System;
using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bfm.Diet.Core.EntityFrameworkCore.Audit
{
    public static class EntityAuditingHelper
    {
        public static void SetCreationAudit(EntityEntry entityObject, int? userId)
        {
            if (!(entityObject.Entity is ICreationAuditableEntity entity)) return;

            entity.KayitTarihi ??= DateTime.UtcNow;

            if (userId.HasValue && entity.Kaydeden == null)
                entity.Kaydeden = userId;
        }

        public static void SetUpdateAudit(EntityEntry entityObject, int? userId)
        {
            if (!(entityObject.Entity is IUpdateAuditableEntity entity)) return;

            entity.GuncellemeTarihi = DateTime.UtcNow;
            if (userId.HasValue)
                entity.Guncelleyen = userId;
        }


        public static void SetSoftDeletionAudit(EntityEntry entityObject)
        {
            if (!(entityObject.Entity is ISoftDeleteAuditableEntity entity)) return;

            entity.Silindi = true;
        }
    }
}