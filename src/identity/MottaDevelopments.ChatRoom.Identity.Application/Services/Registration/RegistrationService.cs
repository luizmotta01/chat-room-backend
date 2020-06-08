using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Domain.DomainEvents;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.Events;
using MottaDevelopments.MicroServices.Application.Services;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<Account> _repository;
        private readonly IMapper _mapper;
        
        public RegistrationService(IRepository<Account> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Register(RegistrationRequest request)
        {
            var account = _mapper.Map<Account>(request);

            account.AddDomainEvent(new NewAccountRegistered(account));
            
            var entity = _repository.Add(account);
            
            var saved =  await _repository.UnitOfWork.SaveEntitiesAsync();
            
            return saved;
        }
    }
}