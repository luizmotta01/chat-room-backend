using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MottaDevelopments.MicroServices.Infrastructure.Core.Factories;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Context
{
    public class IntegrationEventDbContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventDbContext>
    {
        public IntegrationEventDbContext CreateDbContext(string[] args) => 
            new IntegrationEventDbContext(
                new DbContextOptionsBuilder<IntegrationEventDbContext>()
                    .UseSqlServer(ConnectionStringFactory.GetIntegrationEventConnectionStringFromEnvironmentVariables()).Options);
    }
}