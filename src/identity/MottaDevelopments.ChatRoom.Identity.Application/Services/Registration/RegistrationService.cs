using System.Threading.Tasks;
using AutoMapper;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.Events;
using MottaDevelopments.MicroServices.Application.Services;
using MottaDevelopments.MicroServices.Domain.Repository;

namespace MottaDevelopments.ChatRoom.Identity.Application.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<Account> _repository;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly IMapper _mapper;

        public RegistrationService(IRepository<Account> repository, IMapper mapper, IIntegrationEventService integrationEventService)
        {
            _repository = repository;
            _mapper = mapper;
            _integrationEventService = integrationEventService;
        }

        public async Task<bool> Register(RegistrationRequest request)
        {
            var account = _mapper.Map<Account>(request);

            var entity = _repository.Add(account);

            var saved =  await _repository.UnitOfWork.SaveEntitiesAsync();

            if (saved)
                await _integrationEventService.AddAndSaveIntegrationEventAsync(new NewAccountRegistered()
                    {AccountId = entity.Id, Username = entity.Username});

            return saved;
        }
    }
}