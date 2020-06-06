using System.ComponentModel.DataAnnotations;

namespace MottaDevelopments.ChatRoom.Identity.Application.Models
{
    public class AuthenticationRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}