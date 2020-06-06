using System.Threading.Tasks;
using AutoMapper;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
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

            _repository.Add(account);

            return await _repository.UnitOfWork.SaveEntitiesAsync();
           
        }
    }
}