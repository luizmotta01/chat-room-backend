using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MottaDevelopments.MicroServices.Infrastructure.Dapper
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
