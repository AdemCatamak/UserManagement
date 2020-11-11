using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Services
{
    public interface IPasswordHasher
    {
        PasswordHash Hash(Password password);
    }
}