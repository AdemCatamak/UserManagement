using FluentMigrator;
using UserManagement.Infrastructure.DatabaseContext.ConfigModels;

namespace UserManagement.Infrastructure.DatabaseContext.SqlServer.Migrations
{
    [Migration(2)]
    [Tags(nameof(SqlDbTypes.SqlServer))]
    public class _0002_CrateTable_Users : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                  .InSchema("dbo")
                  .WithColumn("Id").AsString().PrimaryKey()
                  .WithColumn("Email").AsString().NotNullable()
                  .WithColumn("PasswordHash").AsString().NotNullable()
                  .WithColumn("PasswordHashSalt").AsString().NotNullable()
                  .WithColumn("CreatedOn").AsDateTime().NotNullable();

            Create.Index("IX_Users_Email")
                  .OnTable("Users")
                  .InSchema("dbo")
                  .OnColumn("Email")
                  .Unique();
        }

        public override void Down()
        {
            Delete.Table("Users")
                  .InSchema("dbo");
        }
    }
}