using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using UserManagement.Application.UserScenarios.IntegrationEvents;
using UserManagement.Domain.ValueObjects;
using UserManagement.Infrastructure.EmailEngine;

namespace UserManagement.Consumers
{
    public class WelcomeMailerConsumer : IConsumer<UserCreatedIntegrationEvent>
    {
        private readonly IEmailEngine _emailEngine;

        public WelcomeMailerConsumer(IEmailEngine emailEngine)
        {
            _emailEngine = emailEngine;
        }

        public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
        {
            var userCreatedIntegrationEvent = context.Message;

            var mailSender = new Email("welcome-mailer@usermanagement.com");
            const string mailSubject = "Welcome";
            var mailContent = $"Welcome {userCreatedIntegrationEvent.Email},{Environment.NewLine}" +
                              $"Your password is : {userCreatedIntegrationEvent.Password}";

            var emailPost = new EmailPost(mailSender,
                                          userCreatedIntegrationEvent.Email,
                                          mailSubject,
                                          mailContent);

            await _emailEngine.Send(emailPost);
        }
    }

    public class WelcomeMailerConsumer_Definition : ConsumerDefinition<WelcomeMailerConsumer>
    {
        public WelcomeMailerConsumer_Definition()
        {
            EndpointName = "UserManagement.WelcomeMailerConsumerQueue";
        }
    }
}