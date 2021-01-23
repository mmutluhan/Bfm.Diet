using Autofac;
using Bfm.Diet.Core.Security.Jwt;
using Bfm.Diet.Core.Services;

namespace Bfm.Diet.Web.Api.Modules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MailConfiguration>().AsSelf().SingleInstance();
            builder.RegisterType<TokenOptions>().AsSelf().SingleInstance();

            base.Load(builder);
        }
    }
}