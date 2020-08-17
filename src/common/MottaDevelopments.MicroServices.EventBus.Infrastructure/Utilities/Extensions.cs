using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MottaDevelopments.MicroServices.Application.Services;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Context;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Services;
using MottaDevelopments.MicroServices.Infrastructure.Core.Factories;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Utilities
{
    public static class Extensions
    {
        public static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.AddTransient<IIntegrationEventService, IntegrationEventService>();

            return services;
        }

        public static IServiceCollection AddIntegrationEventDbContext(this IServiceCollection services)
        {
            const string AssemblyName = "MottaDevelopments.MicroServices.EventBus.Infrastructure";

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<IntegrationEventDbContext>(options =>
                {
                    var con = ConnectionStringFactory.GetIntegrationEventConnectionStringFromEnvironmentVariables();
                    options.UseSqlServer(con
                        ,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(AssemblyName);

                            sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                        });
                }, ServiceLifetime.Scoped);

            return services;
        }
        
        public static async Task<IServiceProvider> MigrateIntegrationEventDbContext(this IServiceProvider services)
        {
            using var scope = services.GetService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IntegrationEventDbContext>();

            await context.Database.MigrateAsync();

            return services;

        }
    }
}