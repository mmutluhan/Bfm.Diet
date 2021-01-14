using Microsoft.Extensions.DependencyInjection;

namespace Bfm.Diet.Core.Dependency
{
    public interface ICoreModule
    {
        void Load(IServiceCollection services);
    }
}