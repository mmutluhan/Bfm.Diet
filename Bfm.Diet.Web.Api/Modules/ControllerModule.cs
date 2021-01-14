using Autofac; 

namespace Bfm.Diet.Web.Api.Modules
{
    public class ControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           // builder.RegisterType<ActionResultCacheFilter>().AsSelf().SingleInstance();
           base.Load(builder);
        }
    }
}