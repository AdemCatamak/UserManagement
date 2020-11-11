using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using UserManagement.Domain.Services.IntegrationMessageBroker;
using UserManagement.Infrastructure.IntegrationMessageBroker.ConfigModels;
using UserManagement.Infrastructure.IntegrationMessageBroker.MassTransitSection.MassTransitConverters;
using UserManagement.Infrastructure.IntegrationMessageBroker.MassTransitSection.MassTransitObservers;

namespace UserManagement.Infrastructure.IntegrationMessageBroker
{
    public class IntegrationMessageBroker : IIntegrationMessageBroker
    {
        private readonly IBusControl _busControl;
        private readonly ConcurrentQueue<IIntegrationMessage> _integrationMessageQueue;

        public IntegrationMessageBroker(IBusControl busControl)
        {
            _busControl = busControl;
            _integrationMessageQueue = new ConcurrentQueue<IIntegrationMessage>();
        }

        public IReadOnlyCollection<IIntegrationMessage> IntegrationMessages => _integrationMessageQueue.ToList().AsReadOnly();

        public void Add<T>(T message) where T : IIntegrationMessage
        {
            _integrationMessageQueue.Enqueue(message);
        }

        public async Task DistributeAsync(CancellationToken cancellationToken = default)
        {
            while (_integrationMessageQueue.TryDequeue(out var result))
            {
                switch (result)
                {
                    case IIntegrationEvent _:
                        await _busControl.Publish(result, result.GetType(), cancellationToken);
                        break;
                    case IIntegrationCommand _:
                        await _busControl.Send(result, result.GetType(), cancellationToken);
                        break;
                    default:
                        throw new ArgumentException($"{result.GetType()} is not expected type");
                }
            }
        }
    }

    public static class IntegrationMessageBrokerDIExtension
    {
        public static IServiceCollection AddIntegrationMessageBroker(this IServiceCollection serviceCollection, MessageBrokerOption messageBrokerOption, params Assembly[] assemblies)
        {
            void ConfigureBusFactory(IBusFactoryConfigurator busFactoryConfigurator, IBusRegistrationContext busRegistrationContext)
            {
                foreach (var observer in busRegistrationContext.GetServices<IConsumeObserver>()) busFactoryConfigurator.ConnectConsumeObserver(observer);
                foreach (var observer in busRegistrationContext.GetServices<ISendObserver>()) busFactoryConfigurator.ConnectSendObserver(observer);
                foreach (var observer in busRegistrationContext.GetServices<IPublishObserver>()) busFactoryConfigurator.ConnectPublishObserver(observer);

                busFactoryConfigurator.ConfigureJsonSerializer(settings =>
                                                               {
                                                                   settings.Converters.Add(new MassTransitTypeNameHandlingConverter(TypeNameHandling.Auto));
                                                                   return settings;
                                                               });
                busFactoryConfigurator.ConfigureJsonDeserializer(settings =>
                                                                 {
                                                                     settings.Converters.Add(new MassTransitTypeNameHandlingConverter(TypeNameHandling.Auto));
                                                                     return settings;
                                                                 });

                busFactoryConfigurator.UseRetry(busFactoryConfigurator, configurator => configurator.Interval(2, TimeSpan.FromSeconds(3)));
            }

            serviceCollection.AddSingleton(messageBrokerOption);

            serviceCollection.AddSingleton<IConsumeObserver, BasicConsumeObserver>();
            serviceCollection.AddSingleton<ISendObserver, BasicSendObservers>();
            serviceCollection.AddSingleton<IPublishObserver, BasicPublishObservers>();
            serviceCollection.AddMassTransitHostedService();
            serviceCollection.AddMassTransit(serviceCollectionBusConfigurator =>
                                             {
                                                 serviceCollectionBusConfigurator.AddConsumers(assemblies);
                                                 serviceCollectionBusConfigurator.AddBus(provider =>
                                                                                         {
                                                                                             var busControl = messageBrokerOption.BrokerType switch
                                                                                                              {
                                                                                                                  MessageBrokerTypes.RabbitMq => Bus.Factory.CreateUsingRabbitMq(cfg =>
                                                                                                                                                                                 {
                                                                                                                                                                                     cfg.Host($"{messageBrokerOption.HostName}",
                                                                                                                                                                                              messageBrokerOption.VirtualHost,
                                                                                                                                                                                              hst =>
                                                                                                                                                                                              {
                                                                                                                                                                                                  hst.Username(messageBrokerOption.UserName);
                                                                                                                                                                                                  hst.Password(messageBrokerOption.Password);
                                                                                                                                                                                                  hst.UseCluster(clusterConfigurator => { clusterConfigurator.Node($"{messageBrokerOption.HostName}:{messageBrokerOption.Port}"); });
                                                                                                                                                                                              });

                                                                                                                                                                                     ConfigureBusFactory(cfg, provider);
                                                                                                                                                                                     cfg.ConfigureEndpoints(provider);
                                                                                                                                                                                 }),
                                                                                                                  _ => throw new ArgumentOutOfRangeException()
                                                                                                              };


                                                                                             return busControl;
                                                                                         });
                                             });

            serviceCollection.TryAddScoped<IIntegrationMessageBroker, IntegrationMessageBroker>();
            return serviceCollection;
        }
    }
}