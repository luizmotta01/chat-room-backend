using System;

namespace MottaDevelopments.MicroServices.Domain.EnvironmentVariables
{
    public class DatabaseEnvironmentVariables
    {
        public static string DbHostPlaceholder = "__DB_HOST__";
        
        public static string DbNamePlaceholder = "__DB_NAME__";

        public static string SaPasswordPlaceholder = "__SA_PASSWORD__";

        public string DbHost { get; set; }

        public string DbName { get; set; }

        public string SaPassword { get; set; }

        public static DatabaseEnvironmentVariables GetDatabaseEnvironmentVariables()
        {
            return new DatabaseEnvironmentVariables
            {
                DbHost = Environment.GetEnvironmentVariable(DbHostPlaceholder),
                DbName = Environment.GetEnvironmentVariable(DbNamePlaceholder),
                SaPassword = Environment.GetEnvironmentVariable(SaPasswordPlaceholder)
            };
        }

    }
}