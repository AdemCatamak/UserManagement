using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UserManagement.Infrastructure.IntegrationMessageBroker.MassTransitSection.MassTransitObservers
{
    public class BasicConsumeObserver : IConsumeObserver
    {
        private readonly ILogger<BasicConsumeObserver> _logger;

        public BasicConsumeObserver(ILogger<BasicConsumeObserver> logger)
        {
            _logger = logger;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            _logger.LogInformation($"{context.ReceiveContext.InputAddress} - Message is consuming - messageId = {context.MessageId}{Environment.NewLine}" +
                                   $"{JsonConvert.SerializeObject(context.Message)}");
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            _logger.LogInformation($"{context.ReceiveContext.InputAddress} - Message is consumed - messageId = {context.MessageId}{Environment.NewLine}");
            return Task.CompletedTask;
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            _logger.LogError(exception, $"{context.ReceiveContext.InputAddress} - Message could not consumed - messageId = {context.MessageId}");
            return Task.CompletedTask;
        }
    }
}