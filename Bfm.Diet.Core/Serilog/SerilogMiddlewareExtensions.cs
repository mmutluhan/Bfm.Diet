using Bfm.Diet.Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Bfm.Diet.Core.Serilog
{
    public static class SerilogMiddlewareExtensions
    { 
        public static IApplicationBuilder AddBfmSerilogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BfmSerilogMiddleware>();
        } 
    }
}