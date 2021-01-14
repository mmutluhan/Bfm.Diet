using Autofac;
using Bfm.Diet.Authorization.Data;
using Bfm.Diet.Authorization.Service;
using Bfm.Diet.Core.Extensions;

namespace Bfm.Diet.Web.Api.Modules
{
    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterContext<AuthorizationDbContext>();
            base.Load(builder);
        }
    }
}