using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MottaDevelopments.ChatRoom.Identity.Application.Commands;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Authentication;
using MottaDevelopments.MicroServices.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Application.CommandHandlers
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, Response<AuthenticationResponse>>
    {
        private readonly IAuthenticationService _authenticationService;
        
        public AuthenticateCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Response<AuthenticationResponse>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {

            var authenticationResponse = await _authenticationService.Authenticate(request.Payload, request.IpAddress);
            
            var response = new Response<AuthenticationResponse>(authenticationResponse);

            if (!(authenticationResponse is null))
                return response;

            response.StatusCode = HttpStatusCode.Unauthorized;

            response.Messages.Add("No account was found with the informed credentials.");

            return response;
        }
    }
}