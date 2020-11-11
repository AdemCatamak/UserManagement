using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UserManagement.Domain.Services.DomainMessageBroker
{
    public abstract class DomainEventHolder
    {
        private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new ConcurrentQueue<IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList().AsReadOnly();

        protected void AddDomainEvent<T>(T domainEvent) where T : IDomainEvent
        {
            _domainEvents.Enqueue(domainEvent);
        }

        public bool TryRemoveDomainEvent(out IDomainEvent domainEvent)
        {
            return _domainEvents.TryDequeue(out domainEvent);
        }
    }
}