using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MottaDevelopments.ChatRoom.Identity.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}