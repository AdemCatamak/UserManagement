using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UserManagement.Infrastructure.IntegrationMessageBroker.MassTransitSection.MassTransitObservers
{
    public class BasicSendObservers : ISendObserver
    {
        private readonly ILogger<BasicSendObservers> _logger;

        public BasicSendObservers(ILogger<BasicSendObservers> logger)
        {
            _logger = logger;
        }


        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            _logger.LogInformation($"{context.DestinationAddress} - Message is sending - messageId={context.MessageId}{Environment.NewLine}"
                                 + $"{JsonConvert.SerializeObject(context.Message)}");
            return Task.CompletedTask;
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            _logger.LogInformation($"{context.DestinationAddress} - Message is sent - messageId={context.MessageId}");
            return Task.CompletedTask;
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            _logger.LogError(exception, $"{context.DestinationAddress} - Message could not sent - messageId={context.MessageId}");
            return Task.CompletedTask;
        }
    }
}