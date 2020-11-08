using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Infrastructure.DatabaseContext.Dtos;

namespace UserManagement.Infrastructure.DatabaseContext.EntityTypeConfigurator
{
    public class OutboxMessageTypeConfigurator : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages", "dbo");
            builder.HasKey(m => m.Id);
        }
    }
}