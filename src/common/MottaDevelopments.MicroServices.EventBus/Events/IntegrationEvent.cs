using System;
using Newtonsoft.Json;

namespace MottaDevelopments.MicroServices.EventBus.Events
{
    public class IntegrationEvent
    {
        [JsonIgnore]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreatedAt { get; private set; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
        }
    }
}