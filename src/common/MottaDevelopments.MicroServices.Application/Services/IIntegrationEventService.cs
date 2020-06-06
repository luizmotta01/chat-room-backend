using System;
using System.Threading.Tasks;
using MottaDevelopments.MicroServices.EventBus.Events;

namespace MottaDevelopments.MicroServices.Application.Services
{
    public interface IIntegrationEventService
    {
        Task AddAndSaveIntegrationEventAsync(IntegrationEvent @event, Guid transactionId = default);

        Task PublishEventsAsync(Guid transactionId);
    }
}