using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MottaDevelopments.MicroServices.EventBus.Entities;
using MottaDevelopments.MicroServices.EventBus.Events;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Services
{
    public interface IIntegrationEventPersistenceService
    {
        Task<IEnumerable<IntegrationEventEntity>> RetrievePendingEventsToPublishAsync(Guid transactionId);

        Task SaveEventAsync(IntegrationEvent @event, Guid transactionId);

        Task MarkEventAsInProgressAsync(Guid eventId);

        Task MarkEventAsPublishedAsync(Guid eventId);

        Task MarkEventAsFailedAsync(Guid eventId);
    }
}