using System.Threading;
using System.Threading.Tasks;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Aggregates.UserAggregate.Rules
{
    public interface IUserUniqueChecker
    {
        Task<bool> CheckAsync(Email email, CancellationToken cancellationToken);
    }
}