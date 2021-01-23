using Bfm.Diet.Core.Base;
using Bfm.Diet.Core.Session;
using Bfm.Diet.Model.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Bfm.Diet.Model
{
    public class DietDbContext : DbContextBase
    {
        private IBfmSessionInfo _sessionInfo;

        public DietDbContext(DbContextOptions<DietDbContext> options) : base(options)
        {
        }

        public DietDbContext(DbContextOptions<DietDbContext> options, IBfmSessionInfo sessionInfo) : base(options,
            sessionInfo)
        {
            _sessionInfo = sessionInfo;
        }

        public DbSet<SabitTanim> SabitTanimlar { get; set; }
        public DbSet<SabitTanimDetay> SabitTanimDetaylar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SabitTanimConfiguration());
            modelBuilder.ApplyConfiguration(new SabitTanimDetayConfiguration());
        }
    }
}