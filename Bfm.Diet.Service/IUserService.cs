using System.Threading.Tasks;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Core.Attributes;
using Bfm.Diet.Core.Repository;

namespace Bfm.Diet.Service
{
    public interface IUserService : IRepository<Kullanici, int>, IBusinessService
    {
        [Cache(Lifetime = 10)]
        [Log]
        Kullanici GetKullaniciByEMail(string email);

        [Cache(Lifetime = 300)]
        [Log]
        Task<Kullanici> GetKullaniciByEMailAsync(string email);
    }
}