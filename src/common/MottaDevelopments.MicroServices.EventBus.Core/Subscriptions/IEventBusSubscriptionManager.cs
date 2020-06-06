using System;
using System.Collections.Generic;
using MottaDevelopments.MicroServices.EventBus.Core.Handlers;
using MottaDevelopments.MicroServices.EventBus.Events;
using MottaDevelopments.MicroServices.EventBus.Handlers;

namespace MottaDevelopments.MicroServices.EventBus.Core.Subscriptions
{
    public interface IEventBusSubscriptionManager
    {
        event EventHandler<string> OnEventRemoved;

        bool IsEmpty { get; }

        void AddDynamicSubscription<TDynamicEventHandler>(string eventName)
            where TDynamicEventHandler : IDynamicIntegrationEventHandler;

        void AddSubscription<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IIntegrationEventHandler.IIntegrationEventHandler<TIntegrationEvent>;
        
        void RemoveDynamicSubscription<TDynamicEventHandler>(string eventName)
            where TDynamicEventHandler : IDynamicIntegrationEventHandler;
        
        void RemoveSubscription<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IIntegrationEventHandler.IIntegrationEventHandler<TIntegrationEvent>;

        Type GetEventTypeByName(string eventName);

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        
        
        bool HasSubscriptionsForEvent(string eventName);

        IEnumerable<Subscription> GetHandlersForEvent<T>() where T : IntegrationEvent;
        
        IEnumerable<Subscription> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();

        void Clear();

    }
}