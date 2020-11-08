using FluentMigrator;
using UserManagement.Infrastructure.DatabaseContext.ConfigModels;

namespace UserManagement.Infrastructure.DatabaseContext.SqlServer.Migrations
{
    [Migration(1)]
    [Tags(nameof(SqlDbTypes.SqlServer))]
    public class _0001_CrateTable_OutboxMessages : Migration
    {
        public override void Up()
        {
            Create.Table("OutboxMessageStatuses")
                  .InSchema("dbo")
                  .WithColumn("Id").AsInt32().PrimaryKey()
                  .WithColumn("Name").AsString().PrimaryKey()
                ;

            Insert.IntoTable("OutboxMessageStatuses")
                  .InSchema("dbo")
                  .Row(new {Id = 1, Name = "Waiting"})
                  .Row(new {Id = 2, Name = "InProgress"})
                  .Row(new {Id = 3, Name = "Completed"})
                  .Row(new {Id = 4, Name = "Failed"})
                ;

            Create.Table("OutboxMessages")
                  .InSchema("dbo")
                  .WithColumn("Id").AsString().PrimaryKey()
                  .WithColumn("OutboxMessageStatus").AsInt32().NotNullable()
                  .WithColumn("MessagePayload").AsString(int.MaxValue).NotNullable()
                  .WithColumn("Description").AsString(int.MaxValue).Nullable()
                  .WithColumn("CreatedOn").AsDateTime2().Nullable()
                ;

            Create.Index("OutboxMessages_Status")
                  .OnTable("OutboxMessages")
                  .InSchema("dbo")
                  .OnColumn("OutboxMessageStatus")
                  .Ascending()
                ;
        }

        public override void Down()
        {
            Delete.Table("OutboxMessages")
                  .InSchema("dbo");
        }
    }
}