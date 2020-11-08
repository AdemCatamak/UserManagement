using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Infrastructure.IntegrationMessageBroker.Exceptions;

namespace UserManagement.Infrastructure.IntegrationMessageBroker.ConfigModels
{
    public class MessageBrokerConfig
    {
        public int SelectedIndex { get; set; }
        public List<MessageBrokerOption> MessageBrokerOptions { get; set; } = new List<MessageBrokerOption>();

        public MessageBrokerOption SelectedMessageBrokerOption()
        {
            if (MessageBrokerOptions == null)
                throw new ArgumentNullException(nameof(MessageBrokerOptions));

            if (!MessageBrokerOptions.Any())
                throw new ArgumentException($"{nameof(MessageBrokerOptions)} is empty");

            var dropyMessageBrokerOption = MessageBrokerOptions.FirstOrDefault(o => o.Index == SelectedIndex);

            if (dropyMessageBrokerOption == null)
                throw new MessageBrokerOptionNotFoundException(SelectedIndex);

            return dropyMessageBrokerOption;
        }
    }

    public class MessageBrokerOption
    {
        public int Index { get; set; }
        public MessageBrokerTypes BrokerType { get; set; }
        public string BrokerName { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public enum MessageBrokerTypes
    {
        RabbitMq = 1
    }
}