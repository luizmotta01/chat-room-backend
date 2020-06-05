using System;
using Microsoft.Extensions.Configuration;

namespace MottaDevelopments.MicroServices.Application.Factories
{
    public class ConfigurationFactory
    {
        public static IConfiguration GetConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appSettings = $"appsettings{(!string.IsNullOrEmpty(environment) ? $".{environment}" : string.Empty)}.json";

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(appSettings, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}