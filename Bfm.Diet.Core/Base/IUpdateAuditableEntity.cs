using System;

namespace Bfm.Diet.Core.Base
{
    public interface IUpdateAuditableEntity
    {
        DateTime? GuncellemeTarihi { get; set; }
        int? Guncelleyen { get; set; }
    }
}