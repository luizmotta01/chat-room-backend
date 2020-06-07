using System;
using System.Linq;
using System.Reflection;
using Autofac;
using MassTransit;
using MassTransit.AutofacIntegration;
using MassTransit.ConsumeConfigurators;
using MassTransit.QuartzIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.RabbitMqTransport.Topology;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MottaDevelopments.MicroServices.EventBus.Core.Subscriptions;
using MottaDevelopments.MicroServices.EventBus.Extensions.Configuration;
using MottaDevelopments.MicroServices.EventBus.Extensions.Consumers;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Context;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Services;
using Quartz;
using RabbitMQ.Client;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Registrations
{
    public static class EventBusRabbitMqMassTransitModuleRegister
    {
        public static void AddMassTransitModule(this ContainerBuilder builder, EventBusConfigUri configuration)
        {
            AddMassTransitModuleHelper(builder, configuration);
        }
        public static void AddMassTransitModule(this ContainerBuilder builder, EventBusConfig configuration)
        {
            AddMassTransitModuleHelper(builder, configuration);
        }

        private static void AddMassTransitModuleHelper(this ContainerBuilder builder, IEventBusConfigBase configuration)
        {
            builder.AddMassTransit(conf =>
            {
                var consumers = configuration.ConsumersAssembly.GetTypes()
                    .Where(type => typeof(IConsumer).IsAssignableFrom(type)
                    && !typeof(IEventBusConsumerWithoutConcurrentMessage).IsAssignableFrom(type)
                    && !typeof(IEventBusDirectConsumer).IsAssignableFrom(type)).ToList();

                if (consumers.Any())
                {
                    consumers.ForEach(consumer => conf.AddConsumer(consumer));
                }

                #region ConsumersWithoutConcurrentMessage
                
                var consumersWithoutConcurrentMessage = configuration.ConsumersAssembly.GetTypes()
                    .Where(type => typeof(IConsumer).IsAssignableFrom(type)
                    && typeof(IEventBusConsumerWithoutConcurrentMessage).IsAssignableFrom(type)).ToList();

                if (consumersWithoutConcurrentMessage.Any())
                {
                    consumersWithoutConcurrentMessage.ForEach(consumerWithoutConcurrentMessage =>
                    {
                        var genericConsumerDefinition = typeof(GenericConsumerDefinition<>).MakeGenericType(consumerWithoutConcurrentMessage);

                        conf.AddConsumer(consumerWithoutConcurrentMessage, genericConsumerDefinition);
                    });
                }

                #endregion

                conf.AddBus(context => Bus.Factory.CreateUsingRabbitMq(rabbitMqBusFactoryConfigurator =>
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
                            h.Username(configuration.User);
                            h.Password(configuration.Password);
                        });
                    }
                    else
                    {
                        throw new TypeAccessException("Interface type is unknown.");
                    }

                    if (context.Container.IsRegistered(typeof(IScheduler)))
                    {
                        var scheduler = context.Container.Resolve<IScheduler>();

                        var schedulerName = scheduler.SchedulerName;

                        if (!string.IsNullOrWhiteSpace(configuration.SchedulerQueueName))
                            schedulerName = configuration.SchedulerQueueName;

                        rabbitMqBusFactoryConfigurator.ReceiveEndpoint(schedulerName, rabbitMqReceiveEndpointConfigurator =>
                        {
                            // For MT4.0, prefetch must be set for Quartz prior to anything else
                            rabbitMqReceiveEndpointConfigurator.PrefetchCount = 10;

                            rabbitMqBusFactoryConfigurator.UseMessageScheduler(rabbitMqReceiveEndpointConfigurator.InputAddress);

                            rabbitMqReceiveEndpointConfigurator.Consumer(() => new ScheduleMessageConsumer(scheduler));
                            rabbitMqReceiveEndpointConfigurator.Consumer(() => new CancelScheduledMessageConsumer(scheduler));
                        });
                    }

                    #region DirectConsumers

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

                                genericMethod?.Invoke(rabbitMqBusFactoryConfigurator, new[] {action});
                            }

                            #endregion

                            #region ConfigureRabbitMqMessagePublishTopology

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

                                genericMethod?.Invoke(rabbitMqBusFactoryConfigurator, new[] {action});
                            }

                            #endregion

                            #region ConfigureExchangeBindingConfigurator

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
                                        new[] {typeof(Action<IExchangeBindingConfigurator>)});

                                    var genericMethod = method.MakeGenericMethod(directConsumer);

                                    var action = function?.Method.Invoke(null, new[] {routingKey});

                                    genericMethod.Invoke(endpoint, new [] {action});
                                });

                            #endregion
                        });
                    }
                    #endregion


                    rabbitMqBusFactoryConfigurator.ConfigureEndpoints(context);
                    //cfg.UseExtensionsLogging(context.Container.Resolve<ILoggerFactory>());
                }));
            });
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

        #region Private Actions
#pragma warning disable IDE0051 // Remove unused private members
        /// <summary>
        /// Used only with Reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        private static void RabbitMqMessageSendTopologyConfigurator<T>(IRabbitMqMessageSendTopologyConfigurator<T> configurator) where T : class
        {
            configurator.UseRoutingKeyFormatter(x => ((IEventBusDirectConsumer)x.Message).RoutingKey);
        }

        /// <summary>
        /// Used only with Reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        private static void RabbitMqMessagePublishTopologyConfigurator<T>(IRabbitMqMessagePublishTopologyConfigurator<T> configurator) where T : class
        {
            configurator.ExchangeType = ExchangeType.Direct;
        }

        /// <summary>
        /// Used only with Reflection
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="callback"></param>
        private static Action<IExchangeBindingConfigurator> ExchangeBindingConfigurator(string routingKey)
        {
            return callback =>
            {
                callback.RoutingKey = routingKey;
                callback.ExchangeType = ExchangeType.Direct;
            };
        }

        /// <summary>
        /// Used only with Reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configure"></param>
        private static void ConsumerConfigurator<T>(IConsumerConfigurator<T> configure) where T : class
        {
            configure.UseConcurrentMessageLimit(1);
        }

        /// <summary>
        /// Used only with Reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        private static void RabbitMqReceiveEndpointConfigurator<T>(IRabbitMqReceiveEndpointConfigurator endpoint) where T : class, IConsumer, new()
        {
            endpoint.Consumer<T>(cc =>
            {
                cc.UseConcurrentMessageLimit(1);
            });
        }

        /// <summary>
        /// Used only with Reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conf"></param>
        private static void ContainerBuilderConfigurator<T>(IContainerBuilderConfigurator conf) where T : class, IConsumer
        {
            conf.AddConsumer<T>().Endpoint(e =>
            {
                e.ConcurrentMessageLimit = 1;
            });
        }
#pragma warning restore IDE0051 // Remove unused private members
        #endregion
    }

}
