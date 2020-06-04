using System;
using System.Collections.Generic;
using MediatR;

namespace MottaDevelopments.MicroServices.Domain.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }

        IReadOnlyCollection<INotification> DomainEvents { get; }
        
        void AddDomainEvent(INotification @event);

        void RemoveDomainEvent(INotification @event);

        void ClearDomainEvents();
    }
}