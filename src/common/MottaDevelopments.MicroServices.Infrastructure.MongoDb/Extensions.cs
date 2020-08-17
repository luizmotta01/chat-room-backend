using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MottaDevelopments.MicroServices.Domain.Entities;

namespace MottaDevelopments.MicroServices.Infrastructure.MongoDb
{
    public static class Extensions
    {
        public static async Task<int> SendDomainEventsAsync(this IMediator mediator, IEntity entity)
        {
            if (entity is null)
                return 0;

            var domainEvents = entity.DomainEvents.ToList();

            entity.ClearDomainEvents();

            var tasks = domainEvents.Select(async domainEvent => await mediator.Publish(domainEvent));

            await Task.WhenAll(tasks);

            return domainEvents.Count;
        }


        public static async Task<int> SendDomainEventsAsync(this IMediator mediator, IEnumerable<IEntity> entities)
        {
            if (entities is null) return 0;

            var entitiesList = entities.ToList();
            
            var countReturn = 0;
            
            foreach (var entity in entitiesList)
            {
                countReturn+= await mediator.SendDomainEventsAsync(entity);
            }

            return countReturn;
        }
    }
}