using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Context;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Factories;

namespace MottaDevelopments.ChatRoom.Identity.Application.Registrations
{
    public static class Extensions
    {
        public static IServiceCollection AddIdentityDbContext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<IdentityDbContext>(nameof(Identity));

            serviceCollection.AddScoped<DbContextBase>(provider => provider.GetService<IdentityDbContext>());

            return serviceCollection;
        }

        public static async Task<IServiceProvider> MigrateDbContext(this IServiceProvider serviceProvider)
        {
            return await serviceProvider.MigrateDbContext<IdentityDbContext>();
        }
    }
}