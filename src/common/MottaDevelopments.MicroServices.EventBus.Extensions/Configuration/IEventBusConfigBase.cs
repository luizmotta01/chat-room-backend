using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MottaDevelopments.MicroServices.EventBus.Extensions.Configuration
{
     public interface IEventBusConfigBase
    {
         string User { get; }

         string Password { get; }

         Assembly ConsumersAssembly { get; }

         Assembly DirectConsumerAssembly { get; }

         string SchedulerQueueName { get; }

         IConfiguration Configuration { get; }

         string DbContextName { get; }

    }
}