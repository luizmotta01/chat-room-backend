using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MottaDevelopments.ChatRoom.Contacts.Api.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

    } 
}