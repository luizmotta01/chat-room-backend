using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Configuration
{
    public abstract class EventBusConfigBase : IEventBusConfigBase
    {
        public string User { get; private set; }
        public string Password { get; private set; }
        public Assembly ConsumersAssembly { get; private set; }
        public Assembly DirectConsumerAssembly { get; private set; }
        public string SchedulerQueueName { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public string DbContextName { get; private set; }

        protected EventBusConfigBase(string user, string password, Assembly consumerAssembly, Assembly directConsumerAssembly, string schedulerQueueName, IConfiguration configuration, string dbContextName)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            ConsumersAssembly = consumerAssembly ?? throw new ArgumentNullException(nameof(consumerAssembly));
            DirectConsumerAssembly = directConsumerAssembly;
            SchedulerQueueName = schedulerQueueName; 
            Configuration = configuration;
            DbContextName = dbContextName;
        }
    }
}