using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Services
{
    public interface IUserIdGenerator
    {
        UserId Generate();
    }
}