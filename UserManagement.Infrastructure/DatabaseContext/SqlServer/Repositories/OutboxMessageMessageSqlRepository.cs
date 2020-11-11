using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Services.IntegrationMessageBroker;
using UserManagement.Infrastructure.DatabaseContext.Dtos;

namespace UserManagement.Infrastructure.DatabaseContext.SqlServer.Repositories
{
    internal class OutboxMessageSqlRepository : IOutboxMessageRepository
    {
        private readonly EfDbContext _dbContext;

        public OutboxMessageSqlRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(IIntegrationMessage integrationMessage, CancellationToken cancellationToken = default)
        {
            var outboxMessage = new OutboxMessage(integrationMessage);
            await _dbContext.AddAsync(outboxMessage, cancellationToken);
        }

        public async Task<(string, IIntegrationMessage)?> SetFirstWaitingMessageInProgressAsync(CancellationToken cancellationToken = default)
        {
            var commandText = $@"
DECLARE @Updated table( [Id] nvarchar(255), [OutboxMessageStatus] int, [MessagePayload] nvarchar(max), [Description] nvarchar(max), [CreatedOn] datetime)
UPDATE [dbo].[OutboxMessages] SET OutboxMessageStatus = {(int) OutboxMessageStatus.InProgress}
OUTPUT INSERTED.Id, INSERTED.OutboxMessageStatus, INSERTED.MessagePayload, INSERTED.Description, INSERTED.CreatedOn
INTO @Updated
WHERE  Id = 
(
    SELECT TOP 1 Id
    FROM [dbo].[OutboxMessages] WITH (UPDLOCK)
    WHERE OutboxMessageStatus = {(int) OutboxMessageStatus.Waiting}
    ORDER BY Id
)
SELECT * FROM @Updated
";

            var outboxMessages = await _dbContext.Set<OutboxMessage>().FromSqlRaw(commandText)
                                                 .ToListAsync(cancellationToken: cancellationToken);

            var outboxMessage = outboxMessages.FirstOrDefault();
            return outboxMessage == null
                       ? ((string Id, IIntegrationMessage integrationMessage)?) null
                       : (outboxMessage.Id, outboxMessage.GetPayload<IIntegrationMessage>());
        }

        public async Task SetCompletedAsync(string messageId, CancellationToken cancellationToken = default)
        {
            var outboxMessage = await _dbContext.Set<OutboxMessage>()
                                                .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken: cancellationToken);
            if (outboxMessage == null) return;

            outboxMessage.SetCompleted();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetFailedAsync(string messageId, string description, CancellationToken cancellationToken = default)
        {
            var outboxMessage = await _dbContext.Set<OutboxMessage>()
                                                .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken: cancellationToken);
            if (outboxMessage == null) return;

            outboxMessage.SetFailed(description);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetFailedJobCountAsync(CancellationToken cancellationToken)
        {
            int failedJobCount = await _dbContext.Set<OutboxMessage>()
                                                 .AsNoTracking()
                                                 .CountAsync(m => m.OutboxMessageStatus == OutboxMessageStatus.Failed, cancellationToken);

            return failedJobCount;
        }
    }
}