

using System;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public sealed class CustomEventBusDirectConsumerAttribute : Attribute
    {
        public string RoutingKey { get; }

        public CustomEventBusDirectConsumerAttribute(string routingKey)
        {
            RoutingKey = routingKey;
        }
    }
}