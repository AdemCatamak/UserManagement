using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Domain.Repositories;
using UserManagement.Infrastructure.DatabaseContext.ConfigModels;
using UserManagement.Infrastructure.DatabaseContext.Migrations;
using UserManagement.Infrastructure.DatabaseContext.SqlServer;
using UserManagement.Infrastructure.DatabaseContext.SqlServer.Migrations;

namespace UserManagement.Infrastructure.DatabaseContext
{
    public static class UserDbContextExtensions
    {
        public static IServiceCollection AddUserDbContext(this IServiceCollection serviceCollection, SqlDbOption sqlDbOption)
        {
            serviceCollection.AddSingleton(sqlDbOption);
            switch (sqlDbOption.SqlDbType)
            {
                case SqlDbTypes.SqlServer:
                    serviceCollection.AddScoped<IDbMigrationEngine, SqlServerDbMigrationEngine>();
                    serviceCollection.AddDbContext<EfDbContext>(builder => builder.UseSqlServer(sqlDbOption.ConnectionStr));
                    serviceCollection.AddScoped<IUserDbContext, UserSqlServerDbContext>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return serviceCollection;
        }
    }
}