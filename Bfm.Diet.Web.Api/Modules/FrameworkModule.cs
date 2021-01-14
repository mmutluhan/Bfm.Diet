using Autofac;
using Bfm.Diet.Core;
using Bfm.Diet.Core.Cache;
using Bfm.Diet.Core.Cache.Memory;
using Bfm.Diet.Core.Cache.Redis;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Interceptor.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
 
namespace Bfm.Diet.Web.Api.Modules
{
    public class FrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           
            builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();
            builder.RegisterType<ScopeResolver>().As<IScopeResolver>().SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(AppSettings).Assembly).Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
            builder.RegisterType<WriteLog>().SingleInstance();
            builder.RegisterType<CacheResults>().SingleInstance();
            builder.RegisterType<RedisCache>().SingleInstance();
            builder.RegisterType<MemoryCache>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
            base.Load(builder);

        }
    }
}