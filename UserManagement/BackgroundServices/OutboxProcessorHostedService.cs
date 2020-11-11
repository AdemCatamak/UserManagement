using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Services.IntegrationMessageBroker;
using Timer = System.Timers.Timer;

namespace UserManagement.BackgroundServices
{
    public class OutboxProcessorHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Timer _timer;

        public OutboxProcessorHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _timer = new Timer();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
            _timer.AutoReset = false;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Enabled = true;

            return Task.CompletedTask;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                bool again;
                do
                {
                    again = ExecuteAsync(_serviceProvider, CancellationToken.None)
                           .ConfigureAwait(false).GetAwaiter().GetResult();
                } while (again);
            }
            finally
            {
                _timer.Enabled = true;
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Elapsed -= TimerOnElapsed;
            _timer.Stop();
            _timer.Dispose();
            return Task.CompletedTask;
        }

        protected async Task<bool> ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var dbContext = provider.GetRequiredService<IUserDbContext>();
                var outboxMessageRepository = dbContext.OutboxMessageRepository;
                var integrationMessageBroker = provider.GetRequiredService<IIntegrationMessageBroker>();

                (string, IIntegrationMessage)? resultTuple = await outboxMessageRepository.SetFirstWaitingMessageInProgressAsync(cancellationToken);
                if (resultTuple == null)
                {
                    return false;
                }

                string messageId = resultTuple.Value.Item1;
                IIntegrationMessage integrationMessage = resultTuple.Value.Item2;

                if (integrationMessage == null) throw new ArgumentNullException(nameof(integrationMessage));

                try
                {
                    integrationMessageBroker.Add(integrationMessage);
                    await integrationMessageBroker.DistributeAsync(cancellationToken);
                    await outboxMessageRepository.SetCompletedAsync(messageId, cancellationToken);
                }
                catch (Exception e)
                {
                    await outboxMessageRepository.SetFailedAsync(messageId, e.ToString(), cancellationToken);
                }

                return true;
            }
        }
    }
}