using System.Threading;
using System.Threading.Tasks;

namespace MottaDevelopments.MicroServices.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}