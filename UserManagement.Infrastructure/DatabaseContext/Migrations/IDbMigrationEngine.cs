namespace UserManagement.Infrastructure.DatabaseContext.Migrations
{
    public interface IDbMigrationEngine
    {
        void MigrateUp();
    }
}