
using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottaDevelopments.MicroServices.Application.Services;

namespace MottaDevelopments.MicroServices.Application.Consul
{
    public static class Extensions
    {
        public static void RegisterConsulServices(this IServiceCollection services)
        {
            var serviceDiscoveryConfiguration = ServiceDiscoveryConfigurationFactory.GetServiceConfig();
            
            var consulClient = CreateConsulClient(serviceDiscoveryConfiguration);
            
            services.AddSingleton(serviceDiscoveryConfiguration);
            
            services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
            
            services.AddSingleton<IConsulClient, ConsulClient>(serviceProvider => consulClient);
        }

        private static ConsulClient CreateConsulClient(ServiceDiscoveryConfiguration serviceConfig) =>
            new ConsulClient(config =>
            {
                config.Address = serviceConfig.ServiceDiscoveryAddress;
            });
    }
}
