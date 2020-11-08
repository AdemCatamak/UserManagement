using System.Threading;
using System.Threading.Tasks;

namespace UserManagement.Domain.Services.DomainMessageBroker
{
    public interface IDomainMessageBroker
    {
        Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken) where T : IDomainEvent;
        Task SendAsync(ICommand domainCommand, CancellationToken cancellationToken);
        Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> domainCommand, CancellationToken cancellationToken);
    }
}