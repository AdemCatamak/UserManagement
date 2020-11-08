using MediatR;

namespace UserManagement.Domain.Services.DomainMessageBroker
{
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
    }
}