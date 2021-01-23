using System.Linq;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Bfm.Diet.Core.Interceptor;
using Bfm.Diet.Core.Services;
using Bfm.Diet.Service;

namespace Bfm.Diet.Web.Api.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ISabitTanimService).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.IsClass && t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CachingInterceptor), typeof(LoggingInterceptor))
                .InstancePerLifetimeScope();


            builder.Register((pi, c) =>
            {
                var sbtService = pi.Resolve<ISabitTanimDetayService>();
                var sbt = sbtService.GetMailSabitleri();
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
            }).As<IMailService>().SingleInstance();
        }
    }
}