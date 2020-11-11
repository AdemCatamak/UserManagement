using MediatR;

namespace UserManagement.Domain.Services.DomainMessageBroker
{
    public interface IDomainEvent : INotification
    {
    }
}