using System.Text;

namespace MottaDevelopments.ChatRoom.Identity.Application.Messages
{
    public class Messages
    {
        protected static readonly StringBuilder Builder = new StringBuilder();

    }

    public class ErrorMessages : Messages
    {
        public static string UsernameConflict(string username) =>
            Builder.Clear().Append($"The username {username} was already taken.").ToString();

        public static string EmailConflict(string email) =>
            Builder.Clear().Append($"The email {email} is already registered.").ToString();

        public static string MustAgreeWithTermsAndPrivacyPolicy() => Builder.Clear().Append($"You must agree with the Terms of Service and Privacy Policy").ToString();

        public static string UnexpectedServerError() => Builder.Clear().Append($"An unexpected error occurred in the server.").ToString();

        public static string NoAccountFound() => Builder.Clear().Append("No account was found with the informed credentials.").ToString();

        public static string WrongPassword() => Builder.Clear().Append("Wrong password").ToString();
    }

    public class SuccessMessages : Messages
    {
        public static string UserSuccessfullyRegistered(string username) =>
            Builder.Clear().Append($"The user {username} was successfully registered").ToString();

        public static string WelcomeMessage(string username) => Builder.Clear().Append($"Welcome {username}!").ToString();
    }
}