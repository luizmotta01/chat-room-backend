using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.MicroServices.Domain.UnitOfWork;
using MottaDevelopments.MicroServices.Infrastructure.Mediator;

namespace MottaDevelopments.MicroServices.Infrastructure.EntityFramework.Context
{
    public class DbContextBase : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public static string MigrationTableName = "__EFMigrationsHistory";

        public DbContextBase(DbContextOptions options, IMediator mediator) : base(options) => _mediator = mediator;

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var saved = await SaveChangesAsync(cancellationToken);

            if (saved > 0)
                await _mediator.SendDomainEventsAsync(this);

            return saved > 0;
        }
    }
}