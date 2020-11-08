using UserManagement.Domain.Services.IntegrationMessageBroker;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.UserScenarios.IntegrationEvents
{
    public class UserCreatedIntegrationEvent : IIntegrationEvent
    {
        public Email Email { get; private set; }
        public Password Password { get; private set; }

        public UserCreatedIntegrationEvent(Email email, Password password)
        {
            Email = email;
            Password = password;
        }
    }
}