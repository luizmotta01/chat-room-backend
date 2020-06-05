using System;

namespace MottaDevelopments.MicroServices.Application.Services
{
    public static class ServiceDiscoveryConfigurationFactory
    {
        public static ServiceDiscoveryConfiguration GetServiceConfig()
        {
            var serviceConfig = new ServiceDiscoveryConfiguration
            {
                ServiceDiscoveryAddress = new Uri(Environment.GetEnvironmentVariable("serviceDiscoveryAddress")),
                ServiceAddress = new Uri(Environment.GetEnvironmentVariable("serviceAddress")),
                ServiceName = Environment.GetEnvironmentVariable("serviceName"),
                ServiceId = Environment.GetEnvironmentVariable("serviceId")
            };

            return serviceConfig;
        }

    }
}