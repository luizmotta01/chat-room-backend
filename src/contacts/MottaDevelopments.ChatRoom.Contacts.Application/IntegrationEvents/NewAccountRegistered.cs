using System;
using MottaDevelopments.MicroServices.EventBus.Events;

// ReSharper disable once CheckNamespace
namespace MottaDevelopments.Events
{
    public class NewAccountRegistered : IntegrationEvent
    {
        public Guid AccountId { get; set; }

        public string Username { get; set; }
    }
}