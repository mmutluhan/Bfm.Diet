using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Bfm.Diet.Core.Serilog
{
    public static class SerilogHostBuilderExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder builder)
        {
            return builder.UseSerilog();
        }

        public static IHostBuilder UserSerilogElasticSearchSupport(this IHostBuilder builder)
        {
            return builder.UseSerilog();
        }


    }
}