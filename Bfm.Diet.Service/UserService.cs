using System.Threading.Tasks;
using Bfm.Diet.Authorization.Data;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Core.EntityFrameworkCore;
using Bfm.Diet.Core.EntityFrameworkCore.Repository;

namespace Bfm.Diet.Service
{
    public class UserService : EfGenericRepositoryBase<AuthorizationDbContext, Kullanici, int>, IUserService
    {
        private readonly IUnitOfWork<AuthorizationDbContext> _unitOfWork;

        public UserService(IDbContextProvider<AuthorizationDbContext> dbContextProvider,
            IUnitOfWork<AuthorizationDbContext> unitOfWork) : base(dbContextProvider)
        {
            _unitOfWork = unitOfWork;
        }


        public override Kullanici Update(Kullanici entity)
        {
            using (_unitOfWork.BeginTransaction())
            {
                var kullanici = base.Update(entity);
                _unitOfWork.Commit();
                return kullanici;
            }
        }

        public override void Delete(int id)
        {
            using (_unitOfWork.BeginTransaction())
            {
                base.Delete(id);
                _unitOfWork.Commit();
            }
        }

        public override async Task<Kullanici> UpdateAsync(Kullanici entity)
        {
            using (_unitOfWork.BeginTransactionAsync())
            {
                var kullanici = await base.UpdateAsync(entity);
                await _unitOfWork.CommitAsync();
                return kullanici;
            }
        }


        public Kullanici GetKullaniciByEMail(string email)
        {
            return base.FirstOrDefault(x => x.Email == email);
        }

        public Task<Kullanici> GetKullaniciByEMailAsync(string email)
        {
            return base.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}