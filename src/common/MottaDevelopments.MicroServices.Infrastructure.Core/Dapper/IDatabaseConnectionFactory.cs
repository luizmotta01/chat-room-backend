using System.Data;

namespace MottaDevelopments.MicroServices.Infrastructure.Core.Dapper
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
