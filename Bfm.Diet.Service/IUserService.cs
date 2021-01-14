using System.Collections.Generic;
using System.Threading.Tasks;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Core.Repository;

namespace Bfm.Diet.Service
{
    public interface IUserService : IRepository<Kullanici, int>
    {
        new List<Kullanici> GetAllList();

        new Kullanici FirstOrDefault(int id);
        new Task<Kullanici> FirstOrDefaultAsync(int id);
    }
}