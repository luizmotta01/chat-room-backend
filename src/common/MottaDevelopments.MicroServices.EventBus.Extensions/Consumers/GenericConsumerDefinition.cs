using MassTransit;
using MassTransit.Definition;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Consumers
{
    public class GenericConsumerDefinition<T> : ConsumerDefinition<T> where T : class, IConsumer
    {
        public GenericConsumerDefinition()
        {
            ConcurrentMessageLimit = 1;
        }
    }
}