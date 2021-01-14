using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bfm.Diet.Authorization.Model.Configuration
{
    public class UserConfiguration : EntityConfigurationBase, IEntityTypeConfiguration<Kullanici>
    {
        public void Configure(EntityTypeBuilder<Kullanici> builder)
        {
            builder.ToTable("kullanici", AuthorizationSchema);
            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.Adi).HasColumnName("adi").IsRequired();
            builder.Property(x => x.Soyadi).HasColumnName("soyadi").IsRequired();
            builder.Property(x => x.Email).HasColumnName("email").IsRequired();
            builder.Property(x => x.PasswordHash).HasColumnName("passwordHash").IsRequired();
            builder.Property(x => x.PasswordSalt).HasColumnName("passwordSalt").IsRequired();
            builder.Property(x => x.Durum).HasColumnName("durum").IsRequired();
            builder.Property(x => x.KayitTarihi).HasColumnName("kayit_tarihi");
            builder.Property(x => x.Kaydeden).HasColumnName("kaydeden");
            builder.Property(x => x.GuncellemeTarihi).HasColumnName("guncelleme_tarihi");
            builder.Property(x => x.Guncelleyen).HasColumnName("guncelleyen");

            builder.HasIndex(o => o.Email).IsUnique();
        }
    }
}