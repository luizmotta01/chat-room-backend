using System.Threading.Tasks;
using MassTransit;
using MediatR;
using MottaDevelopments.Events;

namespace MottaDevelopments.ChatRoom.Contacts.Application.IntegrationEventsHandlers
{
    public class NewAccountRegisteredHandler : IConsumer<NewAccountRegistered>
    {
        private readonly IMediator _mediator;

        public NewAccountRegisteredHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<NewAccountRegistered> context)
        {
            
        }
    }
}