using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.ChatRoom.Contacts.Infrastructure.EntityFramework.Context;
using MottaDevelopments.MicroServices.Infrastructure.EfCore;
using MottaDevelopments.MicroServices.Infrastructure.EfCore.Context;

namespace MottaDevelopments.ChatRoom.Contacts.Application.Registrations
{
    public static class Extensions
    {
        public static IServiceCollection AddContactsDbContext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ContactsDbContext>(nameof(ContactsDbContext));

            serviceCollection.AddScoped<DbContextBase>(provider => provider.GetService<ContactsDbContext>());

            return serviceCollection;
        }
    }
}