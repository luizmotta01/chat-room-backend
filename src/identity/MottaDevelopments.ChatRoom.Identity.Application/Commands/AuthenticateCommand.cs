using MediatR;
using Microsoft.AspNetCore.Mvc;
using MottaDevelopments.ChatRoom.Identity.Application.Commands.Base;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.MicroServices.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Application.Commands
{
    public class AuthenticateCommand : Command, IRequest<Response<AuthenticationResponse>>
    {
        public AuthenticationRequest Payload { get; private set; }

        public AuthenticateCommand(AuthenticationRequest payload, string ipAddress) : base(ipAddress)
        {
            Payload = payload;
        }
    }
}