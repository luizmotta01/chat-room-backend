using System;
using MottaDevelopments.MicroServices.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Contacts.Domain.Entities
{
    public abstract class Account : Entity
    {
        public Guid AccountId { get; private set; }

        public string Username { get; private set; }
    }

}