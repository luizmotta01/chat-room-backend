namespace MottaDevelopments.ChatRoom.Identity.Application.Commands.Base
{
    public class Command
    {
        public virtual string IpAddress { get; private set; }

        public Command(string ipAddress)
        {
            IpAddress = ipAddress;
        }
    }
}