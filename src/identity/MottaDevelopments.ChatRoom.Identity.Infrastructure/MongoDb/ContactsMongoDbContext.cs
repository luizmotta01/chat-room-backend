using Microsoft.Extensions.Logging;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb.Context;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb.Settings;

namespace MottaDevelopments.ChatRoom.Identity.Infrastructure.MongoDb
{
    public class ContactsMongoDbContext : MongoDbContextBase
    {
        public ContactsMongoDbContext(ILogger<ContactsMongoDbContext> logger, IMongoDbSettings settings) : base(logger, settings)
        {
            
        }

        
    }
}