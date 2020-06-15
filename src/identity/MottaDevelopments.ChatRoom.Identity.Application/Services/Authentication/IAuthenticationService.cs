using System.Threading.Tasks;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.MicroServices.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Response<AuthenticationResponse>> Authenticate(AuthenticationRequest request, string ipAddress);
        
        Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress);

        Task<bool> RevokeToken(string token, string ipAddress);
    }
}