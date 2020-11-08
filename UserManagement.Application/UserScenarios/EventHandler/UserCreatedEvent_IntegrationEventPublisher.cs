using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.UserScenarios.IntegrationEvents;
using UserManagement.Domain.Aggregates.UserAggregate.Events;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Services.DomainMessageBroker;

namespace UserManagement.Application.UserScenarios.EventHandler
{
    public class UserCreatedEvent_IntegrationEventPublisher : IDomainEventHandler<UserCreatedEvent>
    {
        private readonly IUserDbContext _userDbContext;

        public UserCreatedEvent_IntegrationEventPublisher(IUserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }


        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.User;
            var password = notification.Password;

            var userCreatedIntegrationEvent = new UserCreatedIntegrationEvent(user.Email, password);
            var outboxMessageRepository = _userDbContext.OutboxMessageRepository;
            await outboxMessageRepository.AddAsync(userCreatedIntegrationEvent, cancellationToken);
            await _userDbContext.SaveAsync(cancellationToken);
        }
    }
}