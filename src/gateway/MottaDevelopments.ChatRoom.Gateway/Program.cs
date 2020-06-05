using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MottaDevelopments.ChatRoom.Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                            optional: true, reloadOnChange: true)
                        .AddJsonFile($"configuration.{hostContext.HostingEnvironment.EnvironmentName}.json",
                            optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
