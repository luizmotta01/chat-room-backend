using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MottaDevelopments.ChatRoom.Identity.Application.Commands;
using MottaDevelopments.ChatRoom.Identity.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Api.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : IdentityController
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            var response = await _mediator.Send(new AuthenticateCommand(request, IpAddress()));

            return response.StatusCode == HttpStatusCode.Unauthorized
                ? (IActionResult) Unauthorized(response.Messages)
                : Ok(response.Payload);
        }
    }
}