using UserManagement.Domain.Services.DomainMessageBroker;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Aggregates.UserAggregate.Events
{
    public class UserCreatedEvent : IDomainEvent
    {
        public User User { get; private set; }
        public Password Password { get; private set; }

        public UserCreatedEvent(User user, Password password)
        {
            User = user;
            Password = password;
        }
    }
}