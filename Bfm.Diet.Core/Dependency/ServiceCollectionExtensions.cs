using Microsoft.Extensions.DependencyInjection;

namespace Bfm.Diet.Core.Dependency
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, ICoreModule[] modules)
        {
            foreach (var module in modules) module.Load(services);

            return ServiceResolver.Create(services);
        }
    }
}