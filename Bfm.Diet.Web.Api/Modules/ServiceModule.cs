using System.Linq;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Bfm.Diet.Core.Interceptor;
using Bfm.Diet.Core.Interceptor.Attributes;
using Bfm.Diet.Core.Services;
using Bfm.Diet.Service;
using Castle.DynamicProxy;

namespace Bfm.Diet.Web.Api.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            var assembly = typeof(ISabitTanimService).Assembly;
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            var customProxyGenerationOptions = new ProxyGenerationOptions() { Selector = new CustomInterceptionSelector() };
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(customProxyGenerationOptions)
                .InterceptedBy(typeof(WriteLog))
                .InterceptedBy(typeof(CacheResults));

            builder.Register((pi, c) =>
            {
                var sbtService = pi.Resolve<ISabitTanimDetayService>();
                var sbt = sbtService.GetAllList(o => o.SabitTanim.Adi == "MAIL_SABITLERI").ToList();
                var mailConfig = new MailConfiguration();
                if (sbt.Count > 0)
                {
                    var port = sbt.FirstOrDefault(x => x.Kodu == "MAIL_PORT")?.Aciklamasi;
                    if (int.TryParse(port, out var intPort))
                        mailConfig.Port = intPort;

                    mailConfig.MailUsername = sbt.FirstOrDefault(x => x.Kodu == "MAIL_USER_NAME")?.Aciklamasi;
                    mailConfig.MailAdress = sbt.FirstOrDefault(x => x.Kodu == "MAIL_ADDRESS")?.Aciklamasi;
                    mailConfig.MailPassword = sbt.FirstOrDefault(x => x.Kodu == "MAIL_USER_PASSWORD")?.Aciklamasi;
                    mailConfig.Host = sbt.FirstOrDefault(x => x.Kodu == "MAIL_HOST")?.Aciklamasi;
                    mailConfig.UseSsl = sbt.FirstOrDefault(x => x.Kodu == "USE_SSL")?.Aciklamasi == "1";
                    mailConfig.UseDefaultCredentials =
                        sbt.FirstOrDefault(x => x.Kodu == "USE_DEF_CRED")?.Aciklamasi == "1";
                }

                return new MailService(mailConfig);
            }).As<IMailService>();
        }
    }
}