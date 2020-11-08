using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UserManagement.Application.Services;
using UserManagement.Application.UserScenarios.Rules;
using UserManagement.BackgroundServices;
using UserManagement.Controllers;
using UserManagement.Controllers.WebMiddleware;
using UserManagement.Domain.Aggregates.UserAggregate.Rules;
using UserManagement.Domain.Services;
using UserManagement.Infrastructure.DatabaseContext;
using UserManagement.Infrastructure.DatabaseContext.ConfigModels;
using UserManagement.Infrastructure.DomainEventBroker;
using UserManagement.Infrastructure.EmailEngine;
using UserManagement.Infrastructure.IntegrationMessageBroker;
using UserManagement.Infrastructure.IntegrationMessageBroker.ConfigModels;

namespace UserManagement
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                                       {
                                           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                           options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                                           options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.Default;
                                           options.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
                                           options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                           options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                                           options.SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                                       })
                    .AddApplicationPart(typeof(HomeController).Assembly);

            services.AddHostedService<OutboxProcessorHostedService>();

            #region Swagger

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1", new OpenApiInfo());
                                       c.CustomSchemaIds(type => type.ToString());
                                   });

            #endregion


            var referencedAssembly = typeof(Program).Assembly.GetReferencedAssemblies().Select(Assembly.Load).ToArray();
            services.AddDomainMessageBroker(referencedAssembly);

            var messageBrokerConfig = _configuration.GetSection("MessageBrokerConfig")
                                                    .Get<MessageBrokerConfig>();
            var messageBrokerOption = messageBrokerConfig.SelectedMessageBrokerOption();
            services.AddIntegrationMessageBroker(messageBrokerOption, typeof(Program).Assembly);

            var sqlDbConfig = _configuration.GetSection("SqlDbConfig")
                                            .Get<SqlDbConfig>();
            var sqlDbOption = sqlDbConfig.SelectedDbOption();
            services.AddUserDbContext(sqlDbOption);

            services.AddScoped<IUserUniqueChecker, UserUniqueChecker>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserIdGenerator, UserIdGenerator>();
            services.AddScoped<IEmailEngine, ConsoleEmailEngine>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GeneralExceptionHandlerMiddleware>();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", ""); });

            app.UseRouting();
            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}