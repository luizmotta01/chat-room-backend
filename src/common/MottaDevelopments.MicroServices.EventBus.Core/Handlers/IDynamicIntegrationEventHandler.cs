using System.Threading.Tasks;

namespace MottaDevelopments.MicroServices.EventBus.Core.Handlers
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic @event);
    }
}