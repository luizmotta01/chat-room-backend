using System.Threading.Tasks;
using MottaDevelopments.ChatRoom.Identity.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Registration
{
    public interface IRegistrationService
    {
        Task<bool> Register(RegistrationRequest request);
    }
}