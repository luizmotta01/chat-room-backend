using System;
using MottaDevelopments.MicroServices.EventBus.Extensions.Attributes;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Consumers
{
    internal static class CustomEventBusDirectConsumer
    {
        public static CustomEventBusDirectConsumerAttribute GetAttribute(Type type)
        {
            var attribute = Attribute.GetCustomAttribute(type, typeof(CustomEventBusDirectConsumerAttribute));

            return attribute as CustomEventBusDirectConsumerAttribute;
        }
    }
}