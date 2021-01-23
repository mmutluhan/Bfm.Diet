using System.Diagnostics;
using Bfm.Diet.Core.Cache.Memory;
using Bfm.Diet.Core.Cache.Redis;
using Bfm.Diet.Core.Exceptions;
using Bfm.Diet.Core.Serilog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Bfm.Diet.Core.Dependency.Modules
{
    public class DietCoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton(p => new RedisCache("Bfm_Diet_Core_Cache_Redis"));
            services.AddSingleton(p => new MemoryCache("Bfm_Diet_Core_Cache_Memory"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            services.AddSingleton<BfmSerilogOptions>();

            services.AddSingleton<Stopwatch>();
        }
    }
}