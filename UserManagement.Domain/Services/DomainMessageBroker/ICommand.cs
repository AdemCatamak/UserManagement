using MediatR;

namespace UserManagement.Domain.Services.DomainMessageBroker
{
    public interface ICommand : ICommand<bool>
    {
    }

    public interface ICommand<out T> : IRequest<T>
    {
    }
}