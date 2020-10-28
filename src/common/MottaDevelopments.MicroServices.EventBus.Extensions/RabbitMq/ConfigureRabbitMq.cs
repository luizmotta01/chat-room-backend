using System;
using System.Linq;
using System.Reflection;
using Autofac;
using MassTransit;
using MassTransit.AutofacIntegration;
using MassTransit.QuartzIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.RabbitMqTransport.Topology;
using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.MicroServices.EventBus.Core.Subscriptions;
using MottaDevelopments.MicroServices.EventBus.Extensions.Configuration;
using MottaDevelopments.MicroServices.EventBus.Extensions.Consumers;
using MottaDevelopments.MicroServices.EventBus.Extensions.Registrations;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Context;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Services;
using Quartz;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.RabbitMq
{
    public static class ConfigureRabbitMq
    {
        public static void AddRabbitMq(this ContainerBuilder builder, IEventBusConfigBase configuration)
        {
            builder.AddMassTransit(conf =>
            {
                AddConsumers(configuration, conf);

                #region ConsumersWithoutConcurrentMessage

                AddConsumersWithoutConcurrentMessage(configuration, conf);

                #endregion

                AddBus(configuration, conf);
            });
        }

        private static void AddBus(IEventBusConfigBase configuration, IContainerBuilderConfigurator conf)
        {
            conf.AddBus(context => Bus.Factory.CreateUsingRabbitMq(rabbitMqBusFactoryConfigurator =>
            {
                IRabbitMqHost host = ConfigureHost(configuration, rabbitMqBusFactoryConfigurator);

                AddScheduledConsumers(configuration, context, rabbitMqBusFactoryConfigurator);

                #region DirectConsumers

                AddDirectConsumers(configuration, rabbitMqBusFactoryConfigurator);

                #endregion

                rabbitMqBusFactoryConfigurator.ConfigureEndpoints(context);
            }));
        }

        private static void AddDirectConsumers(IEventBusConfigBase configuration,
            IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
            var directConsumers = configuration.ConsumersAssembly.GetTypes()
                .Where(type => typeof(IConsumer).IsAssignableFrom(type)
                               && typeof(IEventBusDirectConsumer).IsAssignableFrom(type)).ToList();

            if (directConsumers.Any())
            {
                directConsumers.ForEach(directConsumer =>
                {
                    var routingKey = directConsumer.Name;

                    var eventBusDirectConsumerAttribute =
                        CustomEventBusDirectConsumer.GetAttribute(directConsumer);

                    if (eventBusDirectConsumerAttribute != null)
                    {
                        routingKey = eventBusDirectConsumerAttribute.RoutingKey;
                    }

                    #region ConfigureRabbitMqMessageSendTopology

                    {
                        ConfigureMessageSendTopology(rabbitMqBusFactoryConfigurator, directConsumer);
                    }

                    #endregion

                    #region ConfigureRabbitMqMessagePublishTopology

                    {
                        ConfigureMessagePublishTopology(rabbitMqBusFactoryConfigurator, directConsumer);
                    }

                    #endregion

                    #region ConfigureExchangeBinding

                    ConfigureExchangeBinding(rabbitMqBusFactoryConfigurator, directConsumer, routingKey);

                    #endregion
                });
            }
        }

        private static void ConfigureExchangeBinding(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator,
            Type directConsumer, string routingKey)
        {
            rabbitMqBusFactoryConfigurator.ReceiveEndpoint(
                directConsumer.Name.Replace("Consumer", string.Empty), endpoint =>
                {
                    if (endpoint == null) return;

                    endpoint.ConfigureConsumeTopology = false;

                    var configuratorType = typeof(IExchangeBindingConfigurator);
                    var actionType = typeof(Action<>).MakeGenericType(configuratorType);
                    var functionType = typeof(Func<,>).MakeGenericType(typeof(string), actionType);

                    var function = typeof(EventBusRabbitMqMassTransitModuleRegister)
                        .GetMethod("ExchangeBindingConfigurator",
                            BindingFlags.NonPublic | BindingFlags.Static)
                        ?.CreateDelegate(functionType);

                    var method = typeof(IRabbitMqReceiveEndpointConfigurator).GetMethod("Bind", 1,
                        new[] { typeof(Action<IExchangeBindingConfigurator>) });

                    var genericMethod = method.MakeGenericMethod(directConsumer);

                    var action = function?.Method.Invoke(null, new[] { routingKey });

                    genericMethod.Invoke(endpoint, new[] { action });
                });
        }

        private static void ConfigureMessagePublishTopology(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator,
            Type directConsumer)
        {
            var configuratorType =
                typeof(IRabbitMqMessagePublishTopologyConfigurator<>).MakeGenericType(
                    directConsumer);
            var actionType = typeof(Action<>).MakeGenericType(configuratorType);

            var action = typeof(EventBusRabbitMqMassTransitModuleRegister)
                .GetMethod("RabbitMqMessagePublishTopologyConfigurator",
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(directConsumer)
                .CreateDelegate(actionType);

            var method = typeof(IRabbitMqBusFactoryConfigurator).GetMethod("Publish");

            var genericMethod = method?.MakeGenericMethod(directConsumer);

            genericMethod?.Invoke(rabbitMqBusFactoryConfigurator, new[] { action });
        }

        private static void ConfigureMessageSendTopology(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator,
            Type directConsumer)
        {
            var configuratorType =
                typeof(IRabbitMqMessageSendTopologyConfigurator<>).MakeGenericType(directConsumer);
            var actionType = typeof(Action<>).MakeGenericType(configuratorType);

            var action = typeof(EventBusRabbitMqMassTransitModuleRegister)
                .GetMethod("RabbitMqMessageSendTopologyConfigurator",
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(directConsumer)
                .CreateDelegate(actionType);

            var method = typeof(IRabbitMqBusFactoryConfigurator).GetMethod("Send");

            var genericMethod = method?.MakeGenericMethod(directConsumer);

            genericMethod?.Invoke(rabbitMqBusFactoryConfigurator, new[] { action });
        }

        private static void AddScheduledConsumers(IEventBusConfigBase configuration, IRegistrationContext<IComponentContext> context,
            IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
            if (context.Container.IsRegistered(typeof(IScheduler)))
            {
                var scheduler = context.Container.Resolve<IScheduler>();

                var schedulerName = scheduler.SchedulerName;

                if (!string.IsNullOrWhiteSpace(configuration.SchedulerQueueName))
                    schedulerName = configuration.SchedulerQueueName;

                rabbitMqBusFactoryConfigurator.ReceiveEndpoint(schedulerName, rabbitMqReceiveEndpointConfigurator =>
                {
                    rabbitMqReceiveEndpointConfigurator.PrefetchCount = 10;

                    rabbitMqBusFactoryConfigurator.UseMessageScheduler(rabbitMqReceiveEndpointConfigurator.InputAddress);

                    rabbitMqReceiveEndpointConfigurator.Consumer(() => new ScheduleMessageConsumer(scheduler));
                    rabbitMqReceiveEndpointConfigurator.Consumer(() => new CancelScheduledMessageConsumer(scheduler));
                });
            }
        }

        private static IRabbitMqHost ConfigureHost(IEventBusConfigBase configuration,
            IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
            IRabbitMqHost host;
            if (typeof(EventBusConfig) == configuration.GetType())
            {
                var config = (EventBusConfig)configuration;

                host = rabbitMqBusFactoryConfigurator.Host(config.Path, config.VirtualPath, h =>
                {
                    h.Username(configuration.User);
                    h.Password(configuration.Password);
                });
            }
            else if (typeof(EventBusConfigUri) == configuration.GetType())
            {
                host = rabbitMqBusFactoryConfigurator.Host((configuration as EventBusConfigUri)?.HostAddress, h =>
                {
                    //h.UseRetry(r => r.Interval(5, TimeSpan.FromSeconds(1)));
                    h.Username(configuration.User);
                    h.Password(configuration.Password);
                });
            }
            else
            {
                throw new TypeAccessException("Interface type is unknown.");
            }

            return host;
        }

        private static void AddConsumersWithoutConcurrentMessage(IEventBusConfigBase configuration,
            IContainerBuilderConfigurator conf)
        {
            var consumersWithoutConcurrentMessage = configuration.ConsumersAssembly.GetTypes()
                .Where(type => typeof(IConsumer).IsAssignableFrom(type)
                               && typeof(IEventBusConsumerWithoutConcurrentMessage).IsAssignableFrom(type)).ToList();

            if (consumersWithoutConcurrentMessage.Any())
            {
                consumersWithoutConcurrentMessage.ForEach(consumerWithoutConcurrentMessage =>
                {
                    var genericConsumerDefinition =
                        typeof(GenericConsumerDefinition<>).MakeGenericType(consumerWithoutConcurrentMessage);

                    conf.AddConsumer(consumerWithoutConcurrentMessage, genericConsumerDefinition);
                });
            }
        }

        private static void AddConsumers(IEventBusConfigBase configuration, IContainerBuilderConfigurator conf)
        {
            var consumers = configuration.ConsumersAssembly.GetTypes()
                .Where(type => typeof(IConsumer).IsAssignableFrom(type)
                               && !typeof(IEventBusConsumerWithoutConcurrentMessage).IsAssignableFrom(type)
                               && !typeof(IEventBusDirectConsumer).IsAssignableFrom(type)).ToList();

            if (consumers.Any())
            {
                consumers.ForEach(consumer => conf.AddConsumer(consumer));
            }
        }

        public static IServiceCollection AddCommonEventBusServices(this IServiceCollection services,
            string assemblyName)
        {
            services.AddTransient<IIntegrationEventPersistenceService>(serviceProvider =>
                new IntegrationEventPersistenceService(serviceProvider.GetRequiredService<IntegrationEventDbContext>(),
                    assemblyName));

            services.AddSingleton<IEventBusSubscriptionManager, InMemorySubscriptionManager>();

            return services;
        }
    }
}