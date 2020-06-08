using System;
using System.Collections.Generic;
using MediatR;

namespace MottaDevelopments.MicroServices.Domain.Entities
{
    public abstract class Entity : IEntity
    {
        private readonly List<INotification> _domainEvents = new List<INotification>();

        public Guid Id { get; set; }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;
        
        public void AddDomainEvent(INotification @event) => _domainEvents.Add(@event);

        public void RemoveDomainEvent(INotification @event) => _domainEvents.Remove(@event);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}