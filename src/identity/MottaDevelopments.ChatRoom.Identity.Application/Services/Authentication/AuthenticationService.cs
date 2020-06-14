using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Tokens;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IRepository<Account> _repository;
        private readonly IMapper _mapper;
        //private readonly IPasswordHasher _hasher;
        private readonly IPasswordHasher<Account> _hasher;
        
        public AuthenticationService(IRepository<Account> repository, IMapper mapper, IPasswordHasher<Account> hasher)
        {
            _repository = repository;
            _mapper = mapper;
            _hasher = hasher;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, string ipAddress)
        {
            var account = await _repository.FindEntityAsync(acc =>
                (acc.Username == request.Username || acc.Email == request.Email));
            
            if (account == null)    
                return null;

            if (_hasher.VerifyHashedPassword(account, account.Password, request.Password) == PasswordVerificationResult.Failed) 
                return null;
            
            var jwtToken =
                JwtTokenGenerator.GenerateJwtToken(account, Environment.GetEnvironmentVariable("__JWT_SECRET__"));

            var refreshToken = JwtTokenGenerator.GenerateRefreshToken(ipAddress);

            account.RefreshTokens.Add(refreshToken);

            _repository.Update(account);

            await _repository.UnitOfWork.SaveEntitiesAsync();

            return new AuthenticationResponse(account, jwtToken, refreshToken.Token);
        }

        public Task<AuthenticationResponse> RefreshToken(string token, string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RevokeToken(string token, string ipAddress)
        {
            throw new System.NotImplementedException();
        }
    }
}