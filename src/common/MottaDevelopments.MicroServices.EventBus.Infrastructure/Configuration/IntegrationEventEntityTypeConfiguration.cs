using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottaDevelopments.MicroServices.EventBus.Entities;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Configuration
{
    public class IntegrationEventEntityTypeConfiguration : IEntityTypeConfiguration<IntegrationEventEntity>
    {
        public void Configure(EntityTypeBuilder<IntegrationEventEntity> builder)
        {
            builder.Ignore(@event => @event.EventTypeShortName);
            
            builder.Ignore(@event => @event.IntegrationEvent);

            builder.HasKey(@event => @event.EventId);

            builder.Property(@event => @event.EventId).IsRequired();
            
            builder.Property(@event => @event.Content).IsRequired();
            
            builder.Property(@event => @event.CreatedAt).IsRequired();
            
            builder.Property(@event => @event.State).IsRequired();
            
            builder.Property(@event => @event.TimesSent).IsRequired();
            
            builder.Property(@event => @event.EventTypeName).IsRequired();
            
        }
    }
}