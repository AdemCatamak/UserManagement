using System.Threading;
using System.Threading.Tasks;

namespace UserManagement.Domain.Repositories
{
    public interface IUserDbContext
    {
        IOutboxMessageRepository OutboxMessageRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync(CancellationToken cancellationToken);
    }
}