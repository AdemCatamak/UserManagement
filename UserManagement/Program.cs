using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserManagement.Infrastructure.DatabaseContext;
using UserManagement.Infrastructure.DatabaseContext.Migrations;

namespace UserManagement
{
    public class Program
    {
        private const string STARTUP_PROJECT_NAME = "UserManagement";

        public static void Main(string[] args)
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole())
                                      .CreateLogger<Program>();

            try
            {
                logger.LogInformation($"{STARTUP_PROJECT_NAME} Host is creating");

                using (var host = BuildHost(args))
                {
                    logger.LogInformation($"{STARTUP_PROJECT_NAME} Migration is starting");

                    RunMigration(host.Services);

                    logger.LogInformation($"{STARTUP_PROJECT_NAME} Migration is completed");

                    logger.LogInformation($"{STARTUP_PROJECT_NAME} is starting");
                    host.Run();
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
            }
            finally
            {
                logger.LogInformation($"{STARTUP_PROJECT_NAME} is shutting down");
            }
        }

        private static void RunMigration(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var provider = serviceScope.ServiceProvider;
                var tryCount = 1;
                Console.WriteLine($"DbMigration Attempt Number: {tryCount}");
                using (var efDbContext = provider.GetRequiredService<EfDbContext>())
                {
                    again:
                    try
                    {
                        efDbContext.Database.Migrate();
                    }
                    catch (Exception)
                    {
                        tryCount++;
                        if (tryCount >= 4) throw;

                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        goto again;
                    }
                }


                var migrationEngines = provider.GetServices<IDbMigrationEngine>();
                foreach (var migrationEngine in migrationEngines)
                {
                    migrationEngine.MigrateUp();
                }
            }
        }

        private static IHost BuildHost(string[] args)
        {
            var webHost = GetHostBuilder(args).Build();

            return webHost;
        }

        private static IHostBuilder GetHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                                  .ConfigureWebHostDefaults(webBuilder =>
                                                            {
                                                                webBuilder.UseStartup<Startup>()
                                                                          .ConfigureAppConfiguration((hostingContext, config) => { config.AddJsonFile("appsettings.json"); })
                                                                          .ConfigureLogging((host, logging) =>
                                                                                            {
                                                                                                logging.SetMinimumLevel(LogLevel.Information);
                                                                                                logging.ClearProviders();
                                                                                                logging.AddConsole();
                                                                                            })
                                                                    ;
                                                            })
                                  .ConfigureServices((hostContext, services) => { });

            return hostBuilder;
        }
    }
}