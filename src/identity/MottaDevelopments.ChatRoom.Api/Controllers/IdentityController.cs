using Microsoft.AspNetCore.Mvc;

namespace MottaDevelopments.ChatRoom.Identity.Api.Controllers
{
    public class IdentityController : ControllerBase
    {
        protected virtual string IpAddress() =>
            Request.Headers.ContainsKey("X-Forwarded-For")
                ? (string) Request.Headers["X-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}