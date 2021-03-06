using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MottaDevelopments.MicroServices.Application.Factories;
using MottaDevelopments.MicroServices.Application.Logging;
using MottaDevelopments.MicroServices.Application.Polly;
using MottaDevelopments.MicroServices.EventBus.Infrastructure.Utilities;
using Serilog;

namespace MottaDevelopments.ChatRoom.Contacts.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = ConfigurationFactory.GetConfiguration();

            Log.Logger.CreateLogger(configuration, "MottaDevelopments.ChatRoom.Identity.Api");

            var host = CreateHostBuilder(args).Build();

            var policy = Policies.GetAsyncRetryPolicy(Log.Logger);

            await policy.ExecuteAsync(async () => { await host.Services.MigrateIntegrationEventDbContext(); });

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
