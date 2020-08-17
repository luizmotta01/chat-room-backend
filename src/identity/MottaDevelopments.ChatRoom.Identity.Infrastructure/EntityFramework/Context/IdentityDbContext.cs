using MediatR;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.MicroServices.Infrastructure.EfCore.Context;

namespace MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Context
{
    public class IdentityDbContext : DbContextBase
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IMediator mediator) : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(nameof(Identity));
        }
    }
}