using Autofac;
using Microsoft.AspNetCore.Identity;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Authentication;
using MottaDevelopments.ChatRoom.Identity.Application.Services.Registration;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using MottaDevelopments.MicroServices.Domain.Repository;
using MottaDevelopments.MicroServices.Infrastructure.Core.Dapper;
using MottaDevelopments.MicroServices.Infrastructure.EfCore.Repository;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb.Context;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb.Settings;

namespace MottaDevelopments.ChatRoom.Identity.Application.Autofac.Modules
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(componentContext =>
                {
                    var settings = new MongoDbSettings();
                    settings.FromEnvironmentVariables();
                    return settings;
                })
                .As<IMongoDbSettings>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MongoDbContextBase>()
                .As<IMongoDbContextBase>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MongoDbRepository<Account>>()
                .As<IMongoDbRepository<Account>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PasswordHasher<Account>>()
                .As<IPasswordHasher<Account>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DatabaseConnectionFactory>()
                .As<IDatabaseConnectionFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EfCoreRepository<Account>>()
                .As<IEfCoreRepository<Account>>()
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