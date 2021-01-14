using Bfm.Diet.Core.EntityFrameworkCore;
using Bfm.Diet.Core.EntityFrameworkCore.Repository;
using Bfm.Diet.Model;

namespace Bfm.Diet.Service
{
    public class SabitTanimDetayService : EfGenericRepositoryBase<DietDbContext, SabitTanimDetay, int>,
        ISabitTanimDetayService
    {
        public SabitTanimDetayService(IDbContextProvider<DietDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }
    }
}