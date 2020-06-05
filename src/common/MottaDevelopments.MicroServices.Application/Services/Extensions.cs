using System;
using Microsoft.Extensions.Configuration;

namespace MottaDevelopments.MicroServices.Application.Services
{
    public static class Extensions
    {
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