using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Configuration
{
    public class EventBusConfigUri : EventBusConfigBase
    {

        public Uri HostAddress { get; private set; }

        public EventBusConfigUri(Uri hostAddress,
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
                dbContextName) =>
            HostAddress = hostAddress ?? throw new ArgumentNullException(nameof(hostAddress));
    }
}