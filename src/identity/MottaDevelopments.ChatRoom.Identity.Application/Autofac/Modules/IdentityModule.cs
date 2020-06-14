using Autofac;
using Microsoft.AspNetCore.Identity;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Authentication;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Registration;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.MicroServices.Domain.Repository;
using MottaDevelopments.MicroServices.Infrastructure.Dapper;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Repository;

namespace MottaDevelopments.ChatRoom.Identity.Application.Autofac.Modules
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordHasher<Account>>()
                .As<IPasswordHasher<Account>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DatabaseConnectionFactory>()
                .As<IDatabaseConnectionFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Repository<Account>>()
                .As<IRepository<Account>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AuthenticationService>()
                .As<IAuthenticationService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<RegistrationService>()
                .As<IRegistrationService>()
                .InstancePerLifetimeScope();
        }
    }
}