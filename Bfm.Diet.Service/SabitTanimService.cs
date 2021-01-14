using System.Collections.Generic; 
using Bfm.Diet.Core.EntityFrameworkCore;
using Bfm.Diet.Core.EntityFrameworkCore.Repository;
using Bfm.Diet.Core.Interceptor.Attributes;
using Bfm.Diet.Model;

namespace Bfm.Diet.Service
{
    public class SabitTanimService : EfGenericRepositoryBase<DietDbContext, SabitTanim, int>, ISabitTanimService
    {
        public SabitTanimService(IDbContextProvider<DietDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        [CacheResults(Priority = 10)]
        [WriteLog(Priority = 0)]
        public override List<SabitTanim> GetAllList()
        {
            return base.GetAllList();
        }
    }
}