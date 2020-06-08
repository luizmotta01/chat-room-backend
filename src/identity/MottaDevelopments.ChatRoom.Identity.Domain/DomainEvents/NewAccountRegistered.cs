using System;
using MediatR;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Identity.Domain.DomainEvents
{
    public class NewAccountRegistered  : INotification
    {
        public Account Account { get; private set; }

        public NewAccountRegistered(Account account)
        {
            Account = account;
        }
    }
}