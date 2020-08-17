using System;
using System.Threading.Tasks;
using MassTransit;
using MottaDevelopments.MicroServices.Application.Services;
using MottaDevelopments.MicroServices.EventBus.Events;
using MottaDevelopments.MicroServices.Infrastructure.EfCore.Context;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Services
{
    public class IntegrationEventService : IIntegrationEventService
    {
        private readonly DbContextBase _context;

        private readonly IBusControl _busControl;

        private readonly IIntegrationEventPersistenceService _eventPersistenceService;

        public IntegrationEventService(DbContextBase context, IBusControl busControl, IIntegrationEventPersistenceService eventPersistenceService)
        {
            _context = context;
            _busControl = busControl;
            _eventPersistenceService = eventPersistenceService;
        }

        public IntegrationEventService(IBusControl busControl, IIntegrationEventPersistenceService eventPersistenceService)
        {
            _busControl = busControl;
            _eventPersistenceService = eventPersistenceService;
        }



        public async Task AddAndSaveIntegrationEventAsync(IntegrationEvent @event, Guid transactionId = default)
        {
            if (transactionId == default)
            {
                if(_context == null)
                    throw new ArgumentNullException("DbContext");

                await _eventPersistenceService.SaveEventAsync(@event, _context.CurrentTransaction().TransactionId);

                return;
            }

            await _eventPersistenceService.SaveEventAsync(@event, transactionId);
        }

        public async Task PublishEventsAsync(Guid transactionId)
        {
            var pendingEvents = await _eventPersistenceService.RetrievePendingEventsToPublishAsync(transactionId);

            foreach (var eventEntity in pendingEvents)
            {
                try
                {
                    await _eventPersistenceService.MarkEventAsInProgressAsync(eventEntity.EventId);

                    var type = eventEntity.IntegrationEvent.GetType();

                    await _busControl.Publish(eventEntity.IntegrationEvent, type);

                    await _eventPersistenceService.MarkEventAsPublishedAsync(eventEntity.EventId);
                }
                catch (Exception exception)
                {

                    await _eventPersistenceService.MarkEventAsFailedAsync(eventEntity.EventId);
                }
            }
        }
    }
}