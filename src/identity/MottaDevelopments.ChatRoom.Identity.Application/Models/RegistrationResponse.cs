namespace MottaDevelopments.ChatRoom.Identity.Application.Models
{
    public class RegistrationResponse
    {
        public bool Registered { get; private set; }


        public RegistrationResponse()
        {
            
        }
        public RegistrationResponse(bool registered)
        {
            Registered = registered;
        }
    }
}