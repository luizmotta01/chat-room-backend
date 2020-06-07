using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Configuration
{
    public class EventBusConfig : EventBusConfigBase
    {
        public string Path { get; private set; }
        public string VirtualPath { get; private set; }

        public EventBusConfig(string path,
            string virtualPath,
            string user,
            string password,
            Assembly consumerAssembly,
            Assembly directConsumerAssembly = null,
            string schedulerQueueName = null,
            IConfiguration configuration = null,
            string dbContextName = null) 
            : base(user,
            password,
            consumerAssembly,
            directConsumerAssembly,
            schedulerQueueName,
            configuration,
            dbContextName)
        {
            Path = path;
            VirtualPath = virtualPath;
        }
    }
}