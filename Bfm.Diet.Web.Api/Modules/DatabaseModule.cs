using Autofac;
using Bfm.Diet.Core.Extensions;
using Bfm.Diet.Model;
using Microsoft.Extensions.Configuration;

namespace Bfm.Diet.Web.Api.Modules
{
    public class DatabaseModule : Module
    {
        private readonly IConfiguration _configuration;

        public DatabaseModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterContext<DietDbContext>();
        }
    }
}