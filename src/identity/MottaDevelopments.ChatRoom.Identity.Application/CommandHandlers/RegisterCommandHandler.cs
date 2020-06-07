using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MottaDevelopments.ChatRoom.Identity.Application.Commands;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Registration;
using MottaDevelopments.MicroServices.Application.Models;
using MottaDevelopments.MicroServices.Application.Services;

namespace MottaDevelopments.ChatRoom.Identity.Application.CommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<RegistrationResponse>>
    {
        private readonly IRegistrationService _registrationService;

        public RegisterCommandHandler(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<Response<RegistrationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = await _registrationService.Register(request.Payload);

            return new Response<RegistrationResponse>(new RegistrationResponse(response));
        }
    }
}