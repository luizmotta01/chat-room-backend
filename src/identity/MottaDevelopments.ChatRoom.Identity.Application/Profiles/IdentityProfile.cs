using AutoMapper;
using MottaDevelopments.ChatRoom.Identity.Application.Models;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Identity.Application.Profiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<RegistrationRequest, Account>().ReverseMap();

            CreateMap<AuthenticationRequest, Account>().ReverseMap();
        }
    }
}