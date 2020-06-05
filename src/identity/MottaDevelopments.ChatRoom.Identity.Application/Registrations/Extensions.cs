using System;
using System.Threading.Tasks;
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

        public static async Task<IServiceProvider> MigrateDbContext(this IServiceProvider serviceProvide)
        {
            return await serviceProvide.MigrateDbContext<IdentityDbContext>();
        }
    }
}