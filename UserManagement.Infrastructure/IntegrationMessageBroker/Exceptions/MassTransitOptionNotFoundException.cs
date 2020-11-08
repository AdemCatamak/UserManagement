using System;

namespace UserManagement.Infrastructure.IntegrationMessageBroker.Exceptions
{
    public class MessageBrokerOptionNotFoundException : Exception
    {
        public MessageBrokerOptionNotFoundException(int selectedIndex) : base($"Message broker option could not found. [SelectedIndex : {selectedIndex}]")
        {
        }
    }
}