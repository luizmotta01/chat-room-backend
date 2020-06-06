using System;
using System.Linq;
using MottaDevelopments.MicroServices.EventBus.Events;
using MottaDevelopments.MicroServices.EventBus.States;
using Newtonsoft.Json;

namespace MottaDevelopments.MicroServices.EventBus.Entities
{
    public class IntegrationEventEntity
    {
        public Guid EventId { get; private set; }

        public string EventTypeName { get; private set; }

        public IntegrationEvent IntegrationEvent { get; private set; }

        public IntegrationEventState State { get; set; }

        public int TimesSent { get; set; }

        public DateTime CreatedAt { get; private set; }

        public string Content { get; private set; }

        public string TransactionId { get; private set; }

        public string EventTypeShortName => EventTypeName.Split('.').Last();

        private IntegrationEventEntity() { }

        public IntegrationEventEntity(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            CreatedAt = @event.CreatedAt;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = IntegrationEventState.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }

        public IntegrationEventEntity DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;;

            return this;
        }
    }
}