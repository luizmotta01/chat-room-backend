using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MottaDevelopments.MicroServices.Infrastructure.Factories;
using MottaDevelopments.MicroServices.Infrastructure.Mediator;

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