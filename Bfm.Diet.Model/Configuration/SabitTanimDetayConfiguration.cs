using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bfm.Diet.Model.Configuration
{
    public class SabitTanimDetayConfiguration : EntityConfigurationBase, IEntityTypeConfiguration<SabitTanimDetay>
    {
        public void Configure(EntityTypeBuilder<SabitTanimDetay> builder)
        {
            builder.ToTable("sabit_tanim_detay", Schema);
            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).HasColumnName("sabit_tanim_detay_id").IsRequired();
            builder.Property(x => x.SabitTanimId).HasColumnName("sabit_tanim_id").IsRequired();
            builder.Property(x => x.Kodu).HasColumnName("kodu").IsRequired().HasMaxLength(30);
            builder.Property(x => x.Aciklamasi).HasColumnName("aciklamasi").IsRequired().HasMaxLength(100);
            builder.Property(x => x.OzelKod).HasColumnName("ozel_kod").IsRequired(false).HasMaxLength(30);

            /* AUDIT */
            builder.Property(x => x.Kaydeden).HasColumnName("kaydeden");
            builder.Property(x => x.KayitTarihi).HasColumnName("kayit_tarihi");
            builder.Property(x => x.Guncelleyen).HasColumnName("guncelleyen");
            builder.Property(x => x.GuncellemeTarihi).HasColumnName("guncelleme_tarihi");

            /* NAV */
            builder.HasOne(x => x.SabitTanim).WithMany(m => m.SabitTanimDetaylari).HasForeignKey(f => f.SabitTanimId);
        }
    }
}