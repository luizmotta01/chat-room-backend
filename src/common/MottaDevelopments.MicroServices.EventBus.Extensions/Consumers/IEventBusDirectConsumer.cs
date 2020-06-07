namespace MottaDevelopments.MicroServices.EventBus.Extensions.Consumers
{
    public interface IEventBusDirectConsumer
    {
        string RoutingKey { get; }
    }
}