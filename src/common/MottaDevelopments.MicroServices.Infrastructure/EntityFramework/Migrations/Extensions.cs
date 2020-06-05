using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context;

namespace MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Migrations
{
    public static class Extensions
    {
        public static async Task<IServiceProvider> MigrateDbContext<TDbContext>(this IServiceProvider serviceProvider) where TDbContext : DbContextBase
        {
            using var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            await context.Database.MigrateAsync();

            return serviceProvider;
        }
    }
}