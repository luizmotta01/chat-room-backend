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
            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(ConnectionStringFactory.GetConnectionStringFromEnvironmentVariables(),
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);

                        sqlServerOptions.MigrationsHistoryTable(DbContextBase.MigrationTableName, nameof(Identity));
                    });
            }, ServiceLifetime.Scoped);

            serviceCollection.AddScoped<DbContextBase>(provider => provider.GetService<IdentityDbContext>());

            return serviceCollection;
        }

        public static async Task<IServiceProvider> MigrateDbContext(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            await context.Database.MigrateAsync();

            return serviceProvider;
        }
    }
}