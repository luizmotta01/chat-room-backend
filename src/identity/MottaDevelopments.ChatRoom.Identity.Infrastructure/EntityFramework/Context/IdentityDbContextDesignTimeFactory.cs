using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MottaDevelopments.MicroServices.Infrastructure.Core.Factories;
using MottaDevelopments.MicroServices.Infrastructure.Core.Mediator;

namespace MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Context
{
    public class IdentityDbContextDesignTimeFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args) =>
            new IdentityDbContext(new DbContextOptionsBuilder<IdentityDbContext>()
                    .UseSqlServer(ConnectionStringFactory.GetConnectionStringFromEnvironmentVariables())
                    .Options
                , new FakeMediator());
    }
}