using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace MottaDevelopments.MicroServices.Application.Logging
{
    public static class LoggerFactory
    {
        public static Logger CreateLogger(this ILogger logger, IConfiguration configuration, string appName) =>
            new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
    }
}