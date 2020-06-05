using MottaDevelopments.MicroServices.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Identity.Domain.Entities
{
    public class LoginAccount : Entity
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public void ChangePassword(string newPassword) => Password = newPassword;
    }
}