using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MottaDevelopments.ChatRoom.Identity.Domain.DomainEvents;
using MottaDevelopments.Events;
using MottaDevelopments.MicroServices.Application.Services;

namespace MottaDevelopments.ChatRoom.Identity.Application.DomainEventHandlers
{
    public class NewAccountRegisteredHandler : INotificationHandler<NewAccountRegistered>
    {
        private readonly IIntegrationEventService _integrationEventService;

        public NewAccountRegisteredHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService;
        }

        public async Task Handle(NewAccountRegistered notification, CancellationToken cancellationToken)
        {
            await _integrationEventService.AddAndSaveIntegrationEventAsync(new NewAccountNotification
                {AccountId = notification.Account.Id, Username = notification.Account.Username});
        }
    }
}