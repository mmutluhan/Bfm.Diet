using Autofac;
using Bfm.Diet.Core.EntityFrameworkCore;
using Bfm.Diet.Core.EntityFrameworkCore.Repository;
using Bfm.Diet.Core.Repository;

namespace Bfm.Diet.Web.Api.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DbContextProvider<>)).As(typeof(IDbContextProvider<>))
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfGenericRepositoryBase<,>)).As(typeof(IRepository<>))
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(UnitOfWork<>)).As(typeof(IUnitOfWork<>)).InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}