using MediatR;
using MottaDevelopments.ChatRoom.Identity.Application.Commands.Base;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.MicroServices.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Application.Commands
{
    public class RegisterCommand : Command, IRequest<Response<RegistrationResponse>>
    {
        public RegistrationRequest Payload { get; private set; }

        public RegisterCommand(RegistrationRequest payload, string ipAddress) : base(ipAddress)
        {
            Payload = payload;
        }
    }
}