using System;

namespace MottaDevelopments.MicroServices.Application.Consul.Config
{
    public class ServiceDiscoveryConfiguration
    {
        public Uri ServiceDiscoveryAddress { get; set; }

        public Uri ServiceAddress { get; set; }

        public string ServiceName { get; set; }

        public string ServiceId { get; set; }
    }
}