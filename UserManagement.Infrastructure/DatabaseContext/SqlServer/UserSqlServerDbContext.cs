using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Services.DomainMessageBroker;
using UserManagement.Infrastructure.DatabaseContext.SqlServer.Repositories;

namespace UserManagement.Infrastructure.DatabaseContext.SqlServer
{
    public class UserSqlServerDbContext : IUserDbContext
    {
        private readonly EfDbContext _efDbContext;
        private readonly IDomainMessageBroker _domainMessageBroker;

        public UserSqlServerDbContext(EfDbContext efDbContext, IDomainMessageBroker domainMessageBroker)
        {
            _efDbContext = efDbContext;
            _domainMessageBroker = domainMessageBroker;
        }

        public IOutboxMessageRepository OutboxMessageRepository => new OutboxMessageSqlRepository(_efDbContext);
        public IUserRepository UserRepository => new UserRepository(_efDbContext);

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            var domainEventHolders = _efDbContext.ChangeTracker.Entries()
                                                 .Where(x => x.Entity is DomainEventHolder)
                                                 .Select(x => (DomainEventHolder) x.Entity)
                                                 .ToList();


            foreach (var domainEventHolder in domainEventHolders)
            {
                while (domainEventHolder.TryRemoveDomainEvent(out var domainEvent))
                {
                    await _domainMessageBroker.PublishAsync(domainEvent, cancellationToken);
                }
            }

            await _efDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}