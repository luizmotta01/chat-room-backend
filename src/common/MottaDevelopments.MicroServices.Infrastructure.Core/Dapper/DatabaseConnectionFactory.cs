using System.Data;
using Microsoft.Data.SqlClient;
using MottaDevelopments.MicroServices.Infrastructure.Core.Factories;

namespace MottaDevelopments.MicroServices.Infrastructure.Core.Dapper
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public IDbConnection CreateConnection() => new SqlConnection(ConnectionStringFactory.GetConnectionStringFromEnvironmentVariables());
    }
}
