using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MottaDevelopments.ChatRoom.Contacts.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Contacts.Application.Services
{
    public interface IContactsService
    {
        Task<bool> AddNewUser(User user);

        Task<bool> AddNewContact(User user, Contact contact);

        Task<IEnumerable<Contact>> GetUserContacts(Guid userId);

        Task<IEnumerable<Contact>> GetUserContacts(string username);

    }
}