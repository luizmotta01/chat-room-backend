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
    public class RegistrationController : IdentityController
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            var response = await _mediator.Send(new RegisterCommand(request, IpAddress()));
            
            return  Ok(response);
        }
    }
}