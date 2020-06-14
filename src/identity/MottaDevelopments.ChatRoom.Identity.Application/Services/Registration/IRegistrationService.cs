using System.Threading.Tasks;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.MicroServices.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Registration
{
    public interface IRegistrationService
    {
        Task<Response<RegistrationResponse>> Register(RegistrationRequest request);
    }
}