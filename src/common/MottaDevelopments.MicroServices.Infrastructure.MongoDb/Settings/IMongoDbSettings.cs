namespace MottaDevelopments.MicroServices.Infrastructure.MongoDb.Settings
{
    public interface IMongoDbSettings
    {
        string Database { get; }

        string Host { get; }

        int Port { get; }

        string User { get; }

        string Password { get; }

        string ConnectionString { get; }

        IMongoDbSettings FromEnvironmentVariables();

    }
}