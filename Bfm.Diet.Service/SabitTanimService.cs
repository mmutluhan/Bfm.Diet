using System.Collections.Generic;
using Bfm.Diet.Core.Attributes;
using Bfm.Diet.Core.EntityFrameworkCore;
using Bfm.Diet.Core.EntityFrameworkCore.Repository;
using Bfm.Diet.Model;

namespace Bfm.Diet.Service
{
    public class SabitTanimService : EfGenericRepositoryBase<DietDbContext, SabitTanim, int>, ISabitTanimService
    {
        public SabitTanimService(IDbContextProvider<DietDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        [Cache(Lifetime = 60)]
        [Log]
        public override List<SabitTanim> GetAllList()
        {
            return base.GetAllList();
        }
    }
}