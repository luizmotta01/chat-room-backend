using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MottaDevelopments.MicroServices.Domain.UnitOfWork;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb.Settings;

namespace MottaDevelopments.MicroServices.Infrastructure.MongoDb.Context
{
    public class MongoDbContextBase : IMongoDbContextBase
    {
        protected ILogger<MongoDbContextBase> Logger;

        public IMongoDatabase Database { get; }

        public MongoDbContextBase(ILogger<MongoDbContextBase> logger, IMongoDbSettings settings)
        {
            Logger = logger;
            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.Database);
  

        }
    }
}