using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context;

namespace MottaDevelopments.MicroServices.EventBus.Infrastructure.Utilities
{
    public class ResilientTransaction
    {
        private readonly DbContextBase _context;

        private ResilientTransaction(DbContextBase context) => _context = context ?? throw new ArgumentNullException(nameof(context));

        public static ResilientTransaction New(DbContextBase context) => new ResilientTransaction(context);

        public async Task ExecuteAsync(Func<Task> function)
        {
            var strategy = _context.DatabaseFacade.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = _context.DatabaseFacade.BeginTransaction();

                await function();

                transaction.Commit();
            });
        }
    }
}