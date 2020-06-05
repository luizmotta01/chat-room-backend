using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottaDevelopments.MicroServices.Application.Services;

namespace MottaDevelopments.MicroServices.Application.Consul
{
    public static class Extensions
    {
        public static IServiceCollection AddConsulServices(this IServiceCollection serviceCollection)
        {
            var serviceDiscoveryConfiguration = ServiceDiscoveryConfigurationFactory.GetServiceConfig();
            
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
    }
}
