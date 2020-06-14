using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MottaDevelopments.ChatRoom.Identity.Application.Messages;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Domain.DomainEvents;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.MicroServices.Application.Models;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<Account> _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Account> _hasher;

        public RegistrationService(IRepository<Account> repository, IMapper mapper, IPasswordHasher<Account> hasher)
        {
            _repository = repository;
            _mapper = mapper;
            _hasher = hasher;
        }

        public async Task<Response<RegistrationResponse>> Register(RegistrationRequest request)
        {
            var response = new Response<RegistrationResponse>();
            
            var existentAccount = await
                _repository.FindEntityAsync(acc => acc.Username == request.Username || acc.Email == request.Email);

            if (!(existentAccount is null))
            {
                response.StatusCode = HttpStatusCode.Conflict;
                
                if(existentAccount.Username == request.Username)
                    response.Messages.Add(ErrorMessages.UsernameConflict(existentAccount.Username));
                
                if(existentAccount.Email == request.Email)
                    response.Messages.Add(ErrorMessages.EmailConflict(existentAccount.Email));

                return response;
            }
            
            var account = _mapper.Map<Account>(request);
            
            account.ChangePassword(_hasher.HashPassword(account, request.Password));
            
            account.AddDomainEvent(new NewAccountRegistered(account));
            
            var entity = _repository.Add(account);
            
            var saved =  await _repository.UnitOfWork.SaveEntitiesAsync();

            return saved
                ? new Response<RegistrationResponse>(new RegistrationResponse {Username = entity.Username})
                {
                    StatusCode = HttpStatusCode.Created
                }
                : new Response<RegistrationResponse> {StatusCode = HttpStatusCode.InternalServerError};
        }
    }
}