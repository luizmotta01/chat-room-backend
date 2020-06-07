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
using MottaDevelopments.MicroServices.EventBus.Extensions.RabbitMq;
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
            builder.AddRabbitMq(configuration);
        }
        
        private static void RabbitMqMessageSendTopologyConfigurator<T>(IRabbitMqMessageSendTopologyConfigurator<T> configurator) where T : class
        {
            configurator.UseRoutingKeyFormatter(x => ((IEventBusDirectConsumer)x.Message).RoutingKey);
        }


        private static void RabbitMqMessagePublishTopologyConfigurator<T>(IRabbitMqMessagePublishTopologyConfigurator<T> configurator) where T : class
        {
            configurator.ExchangeType = ExchangeType.Direct;
        }


        private static Action<IExchangeBindingConfigurator> ExchangeBindingConfigurator(string routingKey)
        {
            return callback =>
            {
                callback.RoutingKey = routingKey;
                callback.ExchangeType = ExchangeType.Direct;
            };
        }


        private static void ConsumerConfigurator<T>(IConsumerConfigurator<T> configure) where T : class
        {
            configure.UseConcurrentMessageLimit(1);
        }


        private static void RabbitMqReceiveEndpointConfigurator<T>(IRabbitMqReceiveEndpointConfigurator endpoint) where T : class, IConsumer, new()
        {
            endpoint.Consumer<T>(cc =>
            {
                cc.UseConcurrentMessageLimit(1);
            });
        }


        private static void ContainerBuilderConfigurator<T>(IContainerBuilderConfigurator conf) where T : class, IConsumer
        {
            conf.AddConsumer<T>().Endpoint(e =>
            {
                e.ConcurrentMessageLimit = 1;
            });
        }
        
    }

}
