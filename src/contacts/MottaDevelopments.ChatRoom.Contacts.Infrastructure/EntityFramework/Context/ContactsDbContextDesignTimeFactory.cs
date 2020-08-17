using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MottaDevelopments.MicroServices.Infrastructure.Core.Factories;
using MottaDevelopments.MicroServices.Infrastructure.Core.Mediator;

namespace MottaDevelopments.ChatRoom.Contacts.Infrastructure.EntityFramework.Context
{
    public class ContactsDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ContactsDbContext>
    {
        public ContactsDbContext CreateDbContext(string[] args) =>
            new ContactsDbContext(
                new DbContextOptionsBuilder<ContactsDbContext>().UseSqlServer(ConnectionStringFactory
                    .GetConnectionStringFromEnvironmentVariables()).Options, new FakeMediator());
    }
}