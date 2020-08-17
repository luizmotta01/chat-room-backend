using System;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottaDevelopments.MicroServices.Application.Consul.Config;
using MottaDevelopments.MicroServices.Application.Consul.Service;
using MottaDevelopments.MicroServices.Application.Services;

namespace MottaDevelopments.MicroServices.Application.Consul
{
    public static class Extensions
    {
        public static IServiceCollection AddConsulServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var serviceDiscoveryConfiguration = configuration.GetServiceConfig();
            
            var consulClient = CreateConsulClient(serviceDiscoveryConfiguration);
            
            serviceCollection.AddSingleton(serviceDiscoveryConfiguration);
            
            serviceCollection.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
            
            serviceCollection.AddSingleton<IConsulClient, ConsulClient>(serviceProvider => consulClient);

            return serviceCollection;
        }

        private static ConsulClient CreateConsulClient(ServiceDiscoveryConfiguration serviceConfig) =>
            new ConsulClient(config =>
            {
                config.Address = serviceConfig.ServiceDiscoveryAddress;
            });

        
        public static ServiceDiscoveryConfiguration GetServiceConfig(this IConfiguration configuration)
        {
            var serviceConfig = new ServiceDiscoveryConfiguration
            {
                ServiceDiscoveryAddress = configuration.GetValue<Uri>("ServiceConfig:serviceDiscoveryAddress"),
                ServiceAddress = configuration.GetValue<Uri>("ServiceConfig:serviceAddress"),
                ServiceName = configuration.GetValue<string>("ServiceConfig:serviceName"),
                ServiceId = configuration.GetValue<string>("ServiceConfig:serviceId")
            };

            return serviceConfig;
        }
    }


}
