using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MottaDevelopments.MicroServices.Infrastructure.EfCore.Context
{
    public interface IDatabaseContext
    {
        DatabaseFacade DatabaseFacade { get; }

        bool HasTransactionActive { get; }

        IDbContextTransaction CurrentTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitTransactionAsync(IDbContextTransaction transaction);

        void RollbackTransaction();
    }
}