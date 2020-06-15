using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MottaDevelopments.ChatRoom.Identity.Application.Messages;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Tokens;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.MicroServices.Application.Models;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IRepository<Account> _repository;
        private readonly IPasswordHasher<Account> _hasher;
        
        public AuthenticationService(IRepository<Account> repository, IPasswordHasher<Account> hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        public async Task<Response<AuthenticationResponse>> Authenticate(AuthenticationRequest request, string ipAddress)
        {
            var (response, account) = await ValidateUserCredentials(request);

            if (!(response is null) || account is null) return response;

            if (_hasher.VerifyHashedPassword(account, account.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                response = response ?? new Response<AuthenticationResponse>();
                
                response.StatusCode = HttpStatusCode.Unauthorized;
                
                response.Messages.Add(ErrorMessages.WrongPassword());
                
                return response;
            }
            
            var jwtToken =
                JwtTokenGenerator.GenerateJwtToken(account, Environment.GetEnvironmentVariable("__JWT_SECRET__"));

            var refreshToken = JwtTokenGenerator.GenerateRefreshToken(ipAddress);

            account.RefreshTokens.Add(refreshToken);

            _repository.Update(account);

            await _repository.UnitOfWork.SaveEntitiesAsync();

            response =
                new Response<AuthenticationResponse>(new AuthenticationResponse(account, jwtToken, refreshToken.Token))
                {
                    StatusCode = HttpStatusCode.Accepted
                };

            response.Messages.Add(SuccessMessages.WelcomeMessage(account.Username));
            
            return response;
        }

        public Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RevokeToken(string token, string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        private async Task<(Response<AuthenticationResponse>, Account)> ValidateUserCredentials(AuthenticationRequest request)
        {
            var account = await _repository.FindEntityAsync(acc =>
                (acc.Username == request.Username || acc.Email == request.Email));

            if (!(account is null)) return (null, account);

            var response = new Response<AuthenticationResponse> {StatusCode = HttpStatusCode.Unauthorized};
            
            response.Messages.Add(ErrorMessages.NoAccountFound());

            return (response, null);
        }
    }
}