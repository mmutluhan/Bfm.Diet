using System;

namespace Bfm.Diet.Core.Dto
{
    public abstract class DtoBase<TPrimaryKey> : IDtoBase<TPrimaryKey>
    {
        public virtual long? Kaydeden { get; set; }
        public virtual DateTime? KayitTarihi { get; set; }
        public virtual TPrimaryKey Id { get; set; }
    }
}