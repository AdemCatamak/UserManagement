using System.Threading;
using System.Threading.Tasks;
using UserManagement.Domain.Services.IntegrationMessageBroker;

namespace UserManagement.Domain.Repositories
{
    public interface IOutboxMessageRepository
    {
        Task AddAsync(IIntegrationMessage integrationMessage, CancellationToken cancellationToken);
        Task<(string, IIntegrationMessage)?> SetFirstWaitingMessageInProgressAsync(CancellationToken cancellationToken);
        Task SetCompletedAsync(string messageId, CancellationToken cancellationToken = default);
        Task SetFailedAsync(string messageId, string description, CancellationToken cancellationToken);
        Task<int> GetFailedJobCountAsync(CancellationToken cancellationToken);
    }
}