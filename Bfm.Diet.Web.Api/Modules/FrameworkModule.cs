using Autofac;
using Bfm.Diet.Core;
using Bfm.Diet.Core.Cache.Memory;
using Bfm.Diet.Core.Cache.Redis;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Exceptions;
using Bfm.Diet.Core.Interceptor;
using Bfm.Diet.Core.Serilog;
using Bfm.Diet.Core.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bfm.Diet.Web.Api.Modules
{
    public class FrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();
            builder.RegisterType<ScopeResolver>().As<IScopeResolver>().SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<BfmSession>().As<IBfmSessionInfo>();
         
            builder.RegisterAssemblyTypes(typeof(AppSettings).Assembly).Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
            builder.RegisterType<ExceptionHandler>().As<IExceptionHandler>().SingleInstance();
            builder.RegisterType<RedisCache>().SingleInstance();
            builder.RegisterType<MemoryCache>().SingleInstance();

            builder.RegisterType<CachingInterceptor>().SingleInstance();
            builder.RegisterType<LoggingInterceptor>().SingleInstance();
            builder.RegisterType<LoggingInterceptorAsync>().As<LoggingInterceptorAsync>();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
            builder.RegisterType<BfmSerilogMiddleware>();
            base.Load(builder);
        }
    }
}