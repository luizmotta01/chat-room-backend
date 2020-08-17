using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Context;
using MottaDevelopments.MicroServices.Infrastructure.EfCore;
using MottaDevelopments.MicroServices.Infrastructure.EfCore.Context;


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
    }
}