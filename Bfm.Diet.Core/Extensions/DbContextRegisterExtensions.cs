using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Bfm.Diet.Core.Extensions
{
    public static class AutofacExtensions
    {
        public static void RegisterContext<TContext>(this ContainerBuilder builder) where TContext : DbContext
        {
            builder.Register(componentContext =>
                {
                    var serviceProvider = componentContext.Resolve<IServiceProvider>();
                    var configuration = componentContext.Resolve<IConfiguration>();
                    var dbContextOptions =
                        new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>());
                    var connectionString =
                        configuration.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value;
                    var optionsBuilder = new DbContextOptionsBuilder<TContext>(dbContextOptions)
                        .UseApplicationServiceProvider(serviceProvider)
                        .UseNpgsql(connectionString);

                    return optionsBuilder.Options;
                }).As<DbContextOptions<TContext>>()
                .InstancePerLifetimeScope();

            builder.Register(context => context.Resolve<DbContextOptions<TContext>>())
                .As<DbContextOptions>()
                .InstancePerLifetimeScope();


            builder.RegisterType<TContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}