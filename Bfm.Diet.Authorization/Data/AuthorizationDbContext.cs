using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Authorization.Model.Configuration;
using Bfm.Diet.Core.Base;
using Microsoft.EntityFrameworkCore;

namespace Bfm.Diet.Authorization.Data
{
    public class AuthorizationDbContext : DbContextBase
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        {
        }

        public DbSet<Kullanici> Users { get; set; }
        public DbSet<Grup> Groups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}