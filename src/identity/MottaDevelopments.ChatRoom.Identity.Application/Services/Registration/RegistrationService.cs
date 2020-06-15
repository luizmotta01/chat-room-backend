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
            var response = (VerifyAgreement(request) ?? await VerifyUserConflicts(request)) ?? await RegisterUser(request);

            return response;
        }

        private Response<RegistrationResponse> VerifyAgreement(RegistrationRequest request)
        {
            if (request.Agreement)
                return null;

            var response = new Response<RegistrationResponse>();
            
            response.Messages.Add(ErrorMessages.MustAgreeWithTermsAndPrivacyPolicy());

            response.StatusCode = HttpStatusCode.BadRequest;

            return response;
        }

        private async Task<Response<RegistrationResponse>> VerifyUserConflicts(RegistrationRequest request)
        {
            var existentAccount = await
                _repository.FindEntityAsync(acc => acc.Username == request.Username || acc.Email == request.Email);

            if (existentAccount is null)
                return null;

            var response = new Response<RegistrationResponse> {StatusCode = HttpStatusCode.Conflict};
            
            if (existentAccount.Username == request.Username)
                response.Messages.Add(ErrorMessages.UsernameConflict(existentAccount.Username));

            if (existentAccount.Email == request.Email)
                response.Messages.Add(ErrorMessages.EmailConflict(existentAccount.Email));

            return response;
            
        }

        private async Task<Response<RegistrationResponse>> RegisterUser(RegistrationRequest request)
        {
            var response = new Response<RegistrationResponse>();
            
            var account = _mapper.Map<Account>(request);

            account.ChangePassword(_hasher.HashPassword(account, request.Password));

            account.AddDomainEvent(new NewAccountRegistered(account));

            var entity = _repository.Add(account);

            var saved = await _repository.UnitOfWork.SaveEntitiesAsync();

            if (saved)
            {
                response.Messages.Add(SuccessMessages.UserSuccessfullyRegistered(entity.Username));
                response.StatusCode = HttpStatusCode.Created;
            }
            else
            {
                response.Messages.Add(ErrorMessages.UnexpectedServerError());
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;

        }
    }
}