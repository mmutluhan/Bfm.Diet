using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bfm.Diet.Model.Configuration
{
    public class SabitTanimConfiguration : EntityConfigurationBase, IEntityTypeConfiguration<SabitTanim>
    {
        public void Configure(EntityTypeBuilder<SabitTanim> builder)
        {
            builder.ToTable("sabit_tanim", Schema);
            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).HasColumnName("sabit_tanim_id").IsRequired();
            builder.Property(x => x.Aciklama).HasColumnName("aciklama").IsRequired();

            builder.Property(x => x.Kaydeden).HasColumnName("kaydeden");
            builder.Property(x => x.KayitTarihi).HasColumnName("kayit_tarihi");
            builder.Property(x => x.Guncelleyen).HasColumnName("guncelleyen");
            builder.Property(x => x.GuncellemeTarihi).HasColumnName("guncelleme_tarihi");

            builder.HasMany(x => x.SabitTanimDetaylari).WithOne(y => y.SabitTanim).HasForeignKey(f => f.SabitTanimId);
        }
    }
}