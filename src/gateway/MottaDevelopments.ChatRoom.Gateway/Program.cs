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
                        .AddJsonFile(EnvironmentJsonFile(hostContext,"appsettings"), optional: true, reloadOnChange: true)
                        .AddJsonFile(EnvironmentJsonFile(hostContext, "ocelot"), optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static string EnvironmentJsonFile(HostBuilderContext context, string fileName) =>
            string.IsNullOrEmpty(context.HostingEnvironment.EnvironmentName)
                ? $"{fileName}.json"
                : $"{fileName}.{context.HostingEnvironment.EnvironmentName}.json";
    }
}
