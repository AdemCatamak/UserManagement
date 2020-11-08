using System.Threading;
using System.Threading.Tasks;
using UserManagement.Domain.Aggregates.UserAggregate;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<User> GetUserAsync(Email email, CancellationToken cancellationToken);
    }
}