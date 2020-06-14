using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using MottaDevelopments.MicroServices.Infrastructure.Factories;

namespace MottaDevelopments.MicroServices.Infrastructure.Dapper
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public IDbConnection CreateConnection() => new SqlConnection(ConnectionStringFactory.GetConnectionStringFromEnvironmentVariables());
    }
}
