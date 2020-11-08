using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Services
{
    public interface IPasswordGenerator
    {
        Password Generate();
    }
}