
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.MicroServices.Domain.Entities;

namespace MottaDevelopments.MicroServices.Infrastructure.Mediator
{
    public static class Extensions
    {
        public static async Task<int> SendDomainEventsAsync(this IMediator mediator, DbContext context)
        {
            var entities = context.ChangeTracker.Entries<IEntity>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents != null && entityEntry.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = entities.SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
                .ToList();

            entities.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async domainEvent => await mediator.Publish(domainEvent));

            await Task.WhenAll(tasks);

            return domainEvents.Count;
        }
    }
}
