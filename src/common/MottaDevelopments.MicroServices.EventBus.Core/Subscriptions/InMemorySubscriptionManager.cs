using System;
using System.Collections.Generic;
using System.Linq;
using MottaDevelopments.MicroServices.EventBus.Core.Handlers;
using MottaDevelopments.MicroServices.EventBus.Events;
using MottaDevelopments.MicroServices.EventBus.Handlers;

namespace MottaDevelopments.MicroServices.EventBus.Core.Subscriptions
{
    public class InMemorySubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<Subscription>> _handlers;

        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public bool IsEmpty => !_handlers.Keys.Any();

        public InMemorySubscriptionManager()
        {
            _handlers = new Dictionary<string, List<Subscription>>();
            _eventTypes = new List<Type>();
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName)) 
                _handlers.Add(eventName, new List<Subscription>());

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
                throw new ArgumentException($"{handlerType.Name} is already registered for '{eventName}'", nameof(handlerType));

            _handlers[eventName].Add(isDynamic ? Subscription.Dynamic(handlerType) : Subscription.Typed(handlerType));
        }

        private void DoRemoveHandler(string eventName, Subscription subsToRemove)
        {
            if (subsToRemove == null) return;

            _handlers[eventName].Remove(subsToRemove);

            if (_handlers[eventName].Any()) return;
            
            _handlers.Remove(eventName);

            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);

            if (eventType != null) 
                _eventTypes.Remove(eventType);

            RaiseOnEventRemoved(eventName);
        }

        private void RaiseOnEventRemoved(string eventName) => OnEventRemoved?.Invoke(this, eventName);

        private Subscription FindDynamicSubscriptionToRemove<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private Subscription FindSubscriptionToRemove<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IIntegrationEventHandler.IIntegrationEventHandler<TIntegrationEvent> =>
            DoFindSubscriptionToRemove(GetEventKey<TIntegrationEvent>(), typeof(TIntegrationEventHandler));

        private Subscription DoFindSubscriptionToRemove(string eventName, Type handlerType) => !HasSubscriptionsForEvent(eventName) ? null : _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);

        public void AddDynamicSubscription<TDynamicEventHandler>(string eventName) where TDynamicEventHandler : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(TDynamicEventHandler), eventName, true);
        }

        public void AddSubscription<TIntegrationEvent, TIntegrationEventHandler>() where TIntegrationEvent : IntegrationEvent where TIntegrationEventHandler : IIntegrationEventHandler.IIntegrationEventHandler<TIntegrationEvent>
        {
            var eventName = GetEventKey<TIntegrationEvent>();

            DoAddSubscription(typeof(TIntegrationEventHandler), eventName, isDynamic: false);

            if (!_eventTypes.Contains(typeof(TIntegrationEvent))) 
                _eventTypes.Add(typeof(TIntegrationEvent));
        }

        public void RemoveDynamicSubscription<TDynamicEventHandler>(string eventName) where TDynamicEventHandler : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TDynamicEventHandler>(eventName);
            
            DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveSubscription<TIntegrationEvent, TIntegrationEventHandler>() where TIntegrationEvent : IntegrationEvent where TIntegrationEventHandler : IIntegrationEventHandler.IIntegrationEventHandler<TIntegrationEvent>
        {
            var handlerToRemove = FindSubscriptionToRemove<TIntegrationEvent, TIntegrationEventHandler>();
            
            var eventName = GetEventKey<TIntegrationEvent>();
            
            DoRemoveHandler(eventName, handlerToRemove);
        }

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public bool HasSubscriptionsForEvent<TIntegrationEvent>() where TIntegrationEvent : IntegrationEvent
        {
            var key = GetEventKey<TIntegrationEvent>();

            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public IEnumerable<Subscription> GetHandlersForEvent<TIntegrationEvent>() where TIntegrationEvent : IntegrationEvent
        {
            var key = GetEventKey<TIntegrationEvent>();
                
            return GetHandlersForEvent(key);
        }

        public IEnumerable<Subscription> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public string GetEventKey<TIntegrationEvent>() => typeof(TIntegrationEvent).Name;

        public void Clear() => _handlers.Clear();
    }
}