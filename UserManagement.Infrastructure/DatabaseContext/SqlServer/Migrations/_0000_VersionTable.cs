using FluentMigrator.Runner.VersionTableInfo;

namespace UserManagement.Infrastructure.DatabaseContext.SqlServer.Migrations
{
    public class _0000_VersionTable : DefaultVersionTableMetaData
    {
        public override string SchemaName
        {
            get { return "dbo"; }
        }
    }
}