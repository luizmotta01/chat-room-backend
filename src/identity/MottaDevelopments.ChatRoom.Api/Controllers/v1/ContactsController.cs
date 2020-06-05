using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MottaDevelopments.ChatRoom.Identity.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new
            {
                User = "Motta"
            });
        }
    }
}