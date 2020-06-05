using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MottaDevelopments.MicroServices.Infrastructure.Mediator
{
    public class FakeMediator : IMediator
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask as Task<TResponse>;

        public Task<object> Send(object request, CancellationToken cancellationToken = new CancellationToken()) => Task.FromResult<object>(default);

        public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification => Task.CompletedTask;
    }
}