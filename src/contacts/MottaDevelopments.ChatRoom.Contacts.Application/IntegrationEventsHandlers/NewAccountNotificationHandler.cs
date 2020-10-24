using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using MottaDevelopments.ChatRoom.Contacts.Domain.Entities;
using MottaDevelopments.Events;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Contacts.Application.IntegrationEventsHandlers
{
    public class NewAccountNotificationHandler : IConsumer<NewAccountNotification>
    {
        public NewAccountNotificationHandler(ILogger<NewAccountNotificationHandler> logger, IMapper mapper)
        {
            
        }

        public async Task Consume(ConsumeContext<NewAccountNotification> context)
        {
            
        }
    }
}