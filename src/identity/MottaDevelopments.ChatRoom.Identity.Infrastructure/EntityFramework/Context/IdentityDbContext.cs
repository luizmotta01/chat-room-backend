using MediatR;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Configurations;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context;

namespace MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Context
{
    public class IdentityDbContext : DbContextBase
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IMediator mediator) : base(options, mediator)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(nameof(Identity));

            modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
        }
    }
}