using System;

namespace Bfm.Diet.Core.Base
{
    public interface ICreationAuditableEntity
    {
        DateTime? KayitTarihi { get; set; }
        int? Kaydeden { get; set; }
    }
}