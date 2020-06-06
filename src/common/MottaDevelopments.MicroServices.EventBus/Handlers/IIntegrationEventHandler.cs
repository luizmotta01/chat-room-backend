using System.Threading.Tasks;
using MottaDevelopments.MicroServices.EventBus.Events;

namespace MottaDevelopments.MicroServices.EventBus.Handlers
{
    public interface IIntegrationEventHandler
    {
        public interface IIntegrationEventHandler
        {

        }

        public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
            where TIntegrationEvent : IntegrationEvent
        {
            Task Handle(TIntegrationEvent @event);
        }
    }
}