using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MottaDevelopments.ChatRoom.Contacts.Domain.Entities;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Contacts.Application.Services
{
    public class ContactsService : IContactsService
    {
        private readonly IEfCoreRepository<User> _efCoreRepository;

        public ContactsService(IEfCoreRepository<User> efCoreRepository)
        {
            _efCoreRepository = efCoreRepository;
        }

        public async Task<bool> AddNewUser(User user)
        {
            _efCoreRepository.Add(user);

            return await _efCoreRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<bool> AddNewContact(User user, Contact contact)
        {
            var account = await _efCoreRepository.FindEntityAsync(entity => entity.Id == user.Id);

            if(account is null)
                throw new Exception($"User {user.Username} was not found in the database.");
            
            account.Contacts.Add(contact);

            return await _efCoreRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<IEnumerable<Contact>> GetUserContacts(Guid userId)
        {
            var account = await _efCoreRepository.FindEntityAsync(entity => entity.Id == userId);

            return GetUserContacts(account);
        }
        
        public async Task<IEnumerable<Contact>> GetUserContacts(string username)
        {
            var account = await _efCoreRepository.FindEntityAsync(entity => entity.Username == username);

            return GetUserContacts(account);
        }

        private static IEnumerable<Contact> GetUserContacts(User account)
        {
            if (account is null)
                throw new Exception($"No user with the informed id/username was found in the database.");

            return account.Contacts;
        }
    }
}