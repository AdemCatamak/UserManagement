using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Infrastructure.DatabaseContext.ConfigModels;

namespace UserManagement.Infrastructure.DatabaseContext.Migrations
{
    public abstract class BaseDbMigrationEngine : IDbMigrationEngine
    {
        public abstract SqlDbOption SqlDbOption { get; }
        public abstract IReadOnlyList<Assembly> Assemblies { get; }
        public abstract IVersionTableMetaData VersionTableMetaData { get; }

        public void MigrateUp()
        {
            var serviceProvider = CreateServices(SqlDbOption.SqlDbType, SqlDbOption.ConnectionStr, Assemblies.ToArray());

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }

        private IServiceProvider CreateServices(SqlDbTypes sqlDbOptions, string dbConnectionString, Assembly[] assemblies)
        {
            var serviceCollection = new ServiceCollection()
                                   .AddFluentMigratorCore()
                                   .AddLogging(lb => lb.AddFluentMigratorConsole());
            switch (sqlDbOptions)
            {
                case SqlDbTypes.SqlServer:
                    serviceCollection.Configure<RunnerOptions>(opt => { opt.Tags = new[] {SqlDbTypes.SqlServer.ToString()}; });
                    serviceCollection.ConfigureRunner(rb =>
                                                      {
                                                          rb
                                                             .AddSqlServer()
                                                             .WithGlobalConnectionString(dbConnectionString)
                                                             .ScanIn(assemblies).For.Migrations();

                                                          if (VersionTableMetaData != null)
                                                              rb.WithVersionTable(VersionTableMetaData);
                                                      });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sqlDbOptions), sqlDbOptions, null);
            }

            return serviceCollection.BuildServiceProvider(false);
        }
    }
}