using System.Text;

namespace MottaDevelopments.ChatRoom.Identity.Application.Messages
{
    public static class ErrorMessages
    {
        private static readonly StringBuilder Builder = new StringBuilder();

        public static string UsernameConflict(string username) =>
            Builder.Clear().Append($"The username {username} was already taken.").ToString();

        public static string EmailConflict(string email) =>
            Builder.Clear().Append($"The email {email} is already registered.").ToString();

    }
}