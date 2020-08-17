using System;

namespace MottaDevelopments.MicroServices.Infrastructure.MongoDb.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string Database { get; private set; }

        public string Host { get; private set; }

        public int Port { get; private set; }

        public string User { get; private set; }

        public string Password { get; private set; }

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password)) return $"mongodb://{Host}:{Port}";

                return $"mongodb://{User}:{Password}@{Host}:{Port}";
            }
            set => throw new NotImplementedException();
        }

        public IMongoDbSettings FromEnvironmentVariables()
        {
            Database = Environment.GetEnvironmentVariable("__MONGODB_DATABASE__");
            Host = Environment.GetEnvironmentVariable("__MONGODB_HOST__");
            int.TryParse(Environment.GetEnvironmentVariable("__MONGODB_PORT__"), out var port);
            Port = port;
            User = Environment.GetEnvironmentVariable("__MONGODB_USER__");
            Password = Environment.GetEnvironmentVariable("__MONGODB_PASSWORD__");
            return this;
        }


        public MongoDbSettings()
        {

        }

        public MongoDbSettings(string database, string host, int port)
        {
            Database = database;
            Host = host;
            Port = port;
        }

        public MongoDbSettings(string database, string host, int port, string user, string password) : this(database, host, port)
        {
            User = user;
            Password = password;
        }

    }
}