using System.Collections.Generic;

namespace MottaDevelopments.ChatRoom.Contacts.Domain.Entities
{
    public class User : Account
    {
        public ICollection<Contact> Contacts { get; private set; }  = new List<Contact>();
    }
}