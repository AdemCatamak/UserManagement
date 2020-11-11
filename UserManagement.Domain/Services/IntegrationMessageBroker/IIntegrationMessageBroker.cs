using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UserManagement.Domain.Services.IntegrationMessageBroker
{
    public interface IIntegrationMessageBroker
    {
        IReadOnlyCollection<IIntegrationMessage> IntegrationMessages { get; }
        void Add<T>(T message) where T : IIntegrationMessage;
        Task DistributeAsync(CancellationToken cancellationToken = default);
    }
}