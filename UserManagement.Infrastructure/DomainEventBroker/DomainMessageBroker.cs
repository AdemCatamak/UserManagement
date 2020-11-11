using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserManagement.Domain.Services.DomainMessageBroker;

namespace UserManagement.Infrastructure.DomainEventBroker
{
    public class DomainMessageBroker : IDomainMessageBroker
    {
        private readonly IMediator _mediator;

        public DomainMessageBroker(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken) where T : IDomainEvent
        {
            return _mediator.Publish(domainEvent, cancellationToken);
        }

        public Task SendAsync(ICommand commandMessage, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(commandMessage, cancellationToken);
        }

        public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> commandMessage, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(commandMessage, cancellationToken);
        }
    }

    public static class DomainMessageBrokerDIExtension
    {
        public static IServiceCollection AddDomainMessageBroker(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            serviceCollection.AddMediatR(assemblies);
            serviceCollection.TryAddTransient<IDomainMessageBroker, DomainMessageBroker>();
            return serviceCollection;
        }
    }
}