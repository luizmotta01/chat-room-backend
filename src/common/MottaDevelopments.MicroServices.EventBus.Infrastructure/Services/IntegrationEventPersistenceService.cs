using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MassTransit.Contracts;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.MicroServices.EventBus.Entities;
using MottaDevelopments.MicroServices.EventBus.Events;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Context;
using MottaDevelopments.MicroServices.EventBus.States;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Services
{
    public class IntegrationEventPersistenceService : IIntegrationEventPersistenceService
    {
        private readonly IntegrationEventDbContext _context;

        private readonly List<Type> _eventTypes;

        public IntegrationEventPersistenceService(IntegrationEventDbContext context, string assemblyName)
        {
            _context = context;
            _eventTypes = Assembly.Load((assemblyName 
                                         ?? Assembly.GetEntryAssembly()?.FullName) 
                                        ?? throw new InvalidOperationException(nameof(assemblyName)))
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventEntity>> RetrievePendingEventsToPublishAsync(Guid transactionId)
        {
            try
            {
                var events = await _context.IntegrationEvents.Where(@event =>
                        @event.TransactionId == transactionId.ToString() &&
                        @event.State == IntegrationEventState.NotPublished).ToListAsync();
                    
                var ordered = events.OrderBy(@event => @event.CreatedAt);


                var deserialized = ordered.Select(@event =>
                    @event.DeserializeJsonContent(_eventTypes.Find(type => type.Name == @event.EventTypeShortName)));

                return deserialized;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task SaveEventAsync(IntegrationEvent @event, Guid transactionId)
        {
            if(transactionId == null) throw new ArgumentNullException(nameof(transactionId));

            var eventEntity = new IntegrationEventEntity(@event, transactionId);

            _context.IntegrationEvents.Add(eventEntity);

            return _context.SaveChangesAsync();
        }

        public async Task MarkEventAsInProgressAsync(Guid eventId) => await SetEventStatus(eventId, IntegrationEventState.InProgress);

        public async Task MarkEventAsPublishedAsync(Guid eventId) => await SetEventStatus(eventId, IntegrationEventState.Published);

        public async Task MarkEventAsFailedAsync(Guid eventId) => await SetEventStatus(eventId, IntegrationEventState.Failed);

        private async Task<int> SetEventStatus(Guid eventId, IntegrationEventState state)
        {
            var entity = await _context.IntegrationEvents.SingleAsync(@event => @event.EventId == eventId);
            
            entity.State = state;

            if (state == IntegrationEventState.InProgress)
                entity.TimesSent++;

            _context.IntegrationEvents.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}