using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Context;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework;

namespace MottaDevelopments.ChatRoom.Identity.Application.Registrations
{
    public static class Extensions
    {
        public static IServiceCollection AddIdentityDbContext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<IdentityDbContext>(nameof(IdentityDbContext));

            return serviceCollection;
        }
    }
}