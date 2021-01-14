using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bfm.Diet.Core.Dependency
{
    public static class ServiceResolver
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}