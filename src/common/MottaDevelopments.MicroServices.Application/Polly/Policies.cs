using System;
using Polly;
using Polly.Retry;
using Serilog;

namespace MottaDevelopments.MicroServices.Application.Polly
{
    public static class Policies
    {
        public static AsyncRetryPolicy GetAsyncRetryPolicy(ILogger logger) =>
            Policy.Handle<Exception>()
                .WaitAndRetryForeverAsync(retry => TimeSpan.FromSeconds(5),
                    (exception, timeSpan, ctx) =>
                          {
                              logger.Error(exception,
                                  "---> Exception {ExceptionType} with message {Message} detected. StackTrace: {StackTrace} ",
                                   exception.GetType().Name, exception.Message, exception.StackTrace);
                          });
    }
}