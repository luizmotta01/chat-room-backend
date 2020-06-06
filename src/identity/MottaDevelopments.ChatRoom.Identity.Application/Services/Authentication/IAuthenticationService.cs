using System.Threading.Tasks;
using MottaDevelopments.ChatRoom.Identity.Application.Models;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, string ipAddress);
        
        Task<AuthenticationResponse> RefreshToken(string token, string ipAddress);

        Task<bool> RevokeToken(string token, string ipAddress);
    }
}