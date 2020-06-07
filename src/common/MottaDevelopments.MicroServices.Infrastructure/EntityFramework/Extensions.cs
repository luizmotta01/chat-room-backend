
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Factories;

namespace MottaDevelopments.MicroServices.Infrastructure.EntityFramework
{
    public static class Extensions
    {
        public static async Task<IServiceProvider> MigrateDbContext<TDbContext>(this IServiceProvider serviceProvider) where TDbContext : DbContext
        {
            using var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            await context.Database.MigrateAsync();

            return serviceProvider;
        }

        public static IServiceCollection AddDbContext<TDbContext>(this IServiceCollection services, string schema) where TDbContext : DbContextBase
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<TDbContext>(options =>
            {
                options.UseSqlServer(ConnectionStringFactory.GetConnectionStringFromEnvironmentVariables(),
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);

                        sqlServerOptions.MigrationsHistoryTable(DbContextBase.MigrationTableName, schema);
                    });
            }, ServiceLifetime.Scoped);

            return services;
        }
    }
}