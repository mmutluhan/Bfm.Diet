using System.Collections.Generic;
using System.Threading.Tasks;
using Bfm.Diet.Authorization.Data;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Core.EntityFrameworkCore;
using Bfm.Diet.Core.EntityFrameworkCore.Repository;
using Bfm.Diet.Core.Interceptor.Attributes;

namespace Bfm.Diet.Service
{
    public class UserService : EfGenericRepositoryBase<AuthorizationDbContext, Kullanici, int>, IUserService
    {
        public UserService(IDbContextProvider<AuthorizationDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        [WriteLog(Priority = 10)]
        [CacheResults(Priority = 20, Lifetime = 25)]
        public override List<Kullanici> GetAllList()
        {
            return base.GetAllList();
        }

        [WriteLog(Priority = 10)]
        [CacheResults(Priority = 20, Lifetime = 50)]
        public override Kullanici FirstOrDefault(int id)
        {
            return base.FirstOrDefault(id);
        }

        [WriteLog(Priority = 10)]
        [CacheResults(Priority = 20, Lifetime = 50)]
        public override Task<Kullanici> FirstOrDefaultAsync(int id)
        {
            return base.FirstOrDefaultAsync(id);
        }

    }
}