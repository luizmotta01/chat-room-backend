namespace MottaDevelopments.ChatRoom.Identity.Application.Models
{
    public class RegistrationRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool Agreement { get; set; }

    }
}