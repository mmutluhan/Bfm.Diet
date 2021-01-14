using System;
using System.Collections.Generic;

namespace Bfm.Diet.Core.Base
{
    public abstract class ModelBase<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual DateTime? KayitTarihi { get; set; }
        public virtual int? Kaydeden { get; set; }
        public virtual DateTime? GuncellemeTarihi { get; set; }
        public virtual int? Guncelleyen { get; set; }
        public virtual TPrimaryKey Id { get; set; }

        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default)) return true;

            if (typeof(TPrimaryKey) == typeof(int)) return Convert.ToInt32(Id) <= 0;

            if (typeof(TPrimaryKey) == typeof(long)) return Convert.ToInt64(Id) <= 0;

            return false;
        }
    }
}