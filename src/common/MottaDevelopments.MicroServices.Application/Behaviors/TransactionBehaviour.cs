using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MottaDevelopments.MicroServices.Application.Helpers;
using MottaDevelopments.MicroServices.Application.Services;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context;

namespace MottaDevelopments.MicroServices.Application.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly DbContextBase _context;
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public TransactionBehaviour(DbContextBase context, ILogger<TransactionBehaviour<TRequest, TResponse>> logger, IIntegrationEventService integrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _integrationEventService = integrationEventService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);

            var typeName = request.GetGenericTypeName();

            try
            {
                if (_context.HasTransactionActive)
                    return await next();

                var strategy = _context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _context.BeginTransactionAsync();

                    response = await next();

                    await _context.CommitTransactionAsync(transaction);

                    var transactionId = transaction.TransactionId;

                    await _integrationEventService.PublishEventsAsync(transactionId);
                });

                return response;

            }
            catch (Exception exception)
            {
                _logger.LogError("{Message} - {StackTrace}", exception.Message, exception.StackTrace);

                throw;
            }
        }



    }


}