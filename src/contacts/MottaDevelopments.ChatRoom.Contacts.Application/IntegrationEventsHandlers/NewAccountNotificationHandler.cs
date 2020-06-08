using System.Threading.Tasks;
using MassTransit;
using MediatR;
using MottaDevelopments.ChatRoom.Contacts.Domain.Entities;
using MottaDevelopments.Events;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Contacts.Application.IntegrationEventsHandlers
{
    public class NewAccountNotificationHandler : IConsumer<NewAccountNotification>
    {
        
        public NewAccountNotificationHandler()
        {
            
        }

        public async Task Consume(ConsumeContext<NewAccountNotification> context)
        {
            
        }
    }
}