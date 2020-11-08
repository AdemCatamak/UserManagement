using System.Collections.Generic;
using System.Reflection;
using FluentMigrator.Runner.VersionTableInfo;
using UserManagement.Infrastructure.DatabaseContext.ConfigModels;
using UserManagement.Infrastructure.DatabaseContext.Migrations;

namespace UserManagement.Infrastructure.DatabaseContext.SqlServer.Migrations
{
    public class SqlServerDbMigrationEngine : BaseDbMigrationEngine
    {
        public SqlServerDbMigrationEngine(SqlDbOption sqlDbOption)
        {
            SqlDbOption = sqlDbOption;
            VersionTableMetaData = new _0000_VersionTable();
            Assemblies = new[] {typeof(SqlServerDbMigrationEngine).Assembly};
        }

        public override SqlDbOption SqlDbOption { get; }
        public override IReadOnlyList<Assembly> Assemblies { get; }
        public override IVersionTableMetaData VersionTableMetaData { get; }
    }
}