using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bfm.Diet.Authorization.Model.Configuration
{
    public class GroupConfiguration : EntityConfigurationBase, IEntityTypeConfiguration<Grup>
    {
        public void Configure(EntityTypeBuilder<Grup> builder)
        {
            builder.ToTable("grup", AuthorizationSchema);
            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.Adi).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Aciklama).HasColumnName("Description").IsRequired();
        }
    }
}