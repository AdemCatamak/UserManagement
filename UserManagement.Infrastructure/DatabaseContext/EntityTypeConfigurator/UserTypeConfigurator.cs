using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Aggregates.UserAggregate;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Infrastructure.DatabaseContext.EntityTypeConfigurator
{
    public class UserTypeConfigurator : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");
            builder.HasKey(user => user.UserId);
            builder.Property(m => m.UserId)
                   .HasColumnName("Id")
                   .HasConversion(id => id.Value,
                                  s => new UserId(s));

            builder.OwnsOne(m => m.Email,
                            navigationBuilder =>
                            {
                                navigationBuilder.WithOwner();
                                navigationBuilder.Property(x => x.Value)
                                                 .HasColumnName("Email");
                            });

            builder.OwnsOne(m => m.PasswordHash,
                            navigationBuilder =>
                            {
                                navigationBuilder.WithOwner();
                                navigationBuilder.Property(x => x.Value)
                                                 .HasColumnName("PasswordHash");
                                navigationBuilder.Property(x => x.Salt)
                                                 .HasColumnName("PasswordHashSalt")
                                    ;
                            });
        }
    }
}