using UserManagement.Domain.Services.DomainMessageBroker;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.UserScenarios.Commands
{
    public class CreateUserCommand : ICommand<UserId>
    {
        public Email Email { get; private set; }

        public CreateUserCommand(Email email)
        {
            Email = email;
        }
    }
}