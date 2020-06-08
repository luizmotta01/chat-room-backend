using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Factories;
using MottaDevelopments.MicroServices.Infrastructure.Mediator;

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