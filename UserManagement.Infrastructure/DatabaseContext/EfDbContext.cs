using Microsoft.EntityFrameworkCore;

namespace UserManagement.Infrastructure.DatabaseContext
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}