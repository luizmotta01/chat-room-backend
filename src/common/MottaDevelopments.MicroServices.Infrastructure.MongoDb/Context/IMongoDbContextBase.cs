using MongoDB.Driver;

namespace MottaDevelopments.MicroServices.Infrastructure.MongoDb.Context
{
    public interface IMongoDbContextBase
    {
        public IMongoDatabase Database { get; }

    }
}