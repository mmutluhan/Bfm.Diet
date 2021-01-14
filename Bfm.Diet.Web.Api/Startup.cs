using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Autofac;
using AutoMapper;
using Bfm.Diet.Core;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Dependency.Modules;
using Bfm.Diet.Core.Logging;
using Bfm.Diet.Core.Mapper;
using Bfm.Diet.Core.Security;
using Bfm.Diet.Core.Security.Jwt;
using Bfm.Diet.Dto.Authorization.Validator;
using Bfm.Diet.Dto.Mapper;
using Bfm.Diet.Service;
using Bfm.Diet.Web.Api.Modules;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Bfm.Diet.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<UserValidator>());

            var tokenOptions = Configuration.GetSection("AppSettings:TokenOptions").Get<TokenOptions>();

            var sections = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(options => sections.Bind(options));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:3000"));
            });


            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var claimsIdentity = (ClaimsIdentity) context.Principal?.Identity;
                            var sid = int.Parse(
                                claimsIdentity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value
                                ?? throw new InvalidOperationException());
                       
                            var user = await userService.FirstOrDefaultAsync(sid);
                            if (user == null)
                                context.Fail("Unauthorized");
                            Debug.WriteLine($"User :{user?.Id}, {user?.Adi}");
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            var err = context.Exception.ToString();
                            var result = JsonConvert.SerializeObject(new {err});
                            return context.Response.WriteAsync(result);
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromSeconds(1),
                        ValidAudience = tokenOptions.Audience,
                        ValidIssuer = tokenOptions.Issuer,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true
                    };
                });
            services.AddLogging();

            DietMapper.Initialize(new MapperConfiguration(cfg => { cfg.AddProfile<DietMapperProfile>(); }));

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.SwaggerDoc("CoreSwagger", new OpenApiInfo
                {
                    Title = "Bfm Web Api ",
                    Version = "0.1.0.0",
                    Description = "Bfm Web Api on (ASP.NET Core 5)",
                    Contact = new OpenApiContact
                    {
                        Name = "Bfm",
                        Url = new Uri("http://www.Bfm.com"),
                        Email = "Bfm@Bfm.com"
                    },
                    TermsOfService = new Uri("http://www.Bfm.com/terms/")
                });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Add Bearer {token} ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.BfmFileLogger(opt =>
                {
                    Configuration.GetSection("AppSettings:LoggingSettings").Get<LoggingSettings>();
                });
            });
            services.AddOptions();
            services.AddDependencyResolvers(new ICoreModule[]
            {
                new DietCoreModule()
            });
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new DatabaseModule(Configuration));
            builder.RegisterModule(new FrameworkModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new AuthorizationModule());
            builder.RegisterModule(new ControllerModule());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader());

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "Wizard Api Test .Net Core");
                });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}