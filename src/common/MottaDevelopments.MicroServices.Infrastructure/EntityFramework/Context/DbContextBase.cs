using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MottaDevelopments.MicroServices.Domain.UnitOfWork;
using MottaDevelopments.MicroServices.Infrastructure.Mediator;

namespace MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context
{
    public class DbContextBase : DbContext, IUnitOfWork, IDatabaseContext
    {
        private readonly IMediator _mediator;

        private IDbContextTransaction _currentTransaction;
        
        public static string MigrationTableName = "__EFMigrationsHistory";
        
        public DatabaseFacade DatabaseFacade { get; }
        public bool HasTransactionActive => _currentTransaction != null;
        
        public DbContextBase(DbContextOptions options, IMediator mediator) : base(options) => _mediator = mediator;

        private void DisposeCurrentTransaction()
        {
            if (_currentTransaction is null) return;

            _currentTransaction.Dispose();

            _currentTransaction = null;
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var saved = await SaveChangesAsync(cancellationToken);

            if (saved > 0)
                await _mediator.SendDomainEventsAsync(this);

            return saved > 0;
        }
        
        public IDbContextTransaction CurrentTransaction() => _currentTransaction;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if(transaction is null) throw new ArgumentNullException(nameof(transaction));

            if(transaction != _currentTransaction) throw new InvalidOperationException($"{transaction.TransactionId} is not the current transaction.");

            try
            {
                await SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();

                throw;
            }
            finally
            {
                DisposeCurrentTransaction();
            }
        }
        
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.RollbackAsync();
            }
            finally
            {
                DisposeCurrentTransaction();
            }
        }
    }
}