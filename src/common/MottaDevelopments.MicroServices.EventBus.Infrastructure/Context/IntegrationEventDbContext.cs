using Microsoft.EntityFrameworkCore;
using MottaDevelopments.MicroServices.EventBus.Entities;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Configuration;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Context
{
    public class IntegrationEventDbContext : DbContext
    {
        public IntegrationEventDbContext(DbContextOptions<IntegrationEventDbContext> options) : base(options) { }

        public DbSet<IntegrationEventEntity> IntegrationEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfiguration(new IntegrationEventEntityTypeConfiguration());
    }
}