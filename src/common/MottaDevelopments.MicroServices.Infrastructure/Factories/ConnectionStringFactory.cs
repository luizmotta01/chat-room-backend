﻿using MottaDevelopments.MicroServices.Domain.EnvironmentVariables;

namespace MottaDevelopments.MicroServices.Infrastructure.Factories
{
    public static class ConnectionStringFactory
    {
        private static readonly string _defaultMask =
            "Server=__DB_HOST__;Database=__DB_NAME__;User Id=sa;Password=__SA_PASSWORD__;MultipleActiveResultSets=true;";

        public static string GetConnectionStringFromEnvironmentVariables() => _defaultMask.ReplaceEnvironmentVariables(DatabaseEnvironmentVariables.GetDatabaseEnvironmentVariables());

        public static string GetIntegrationEventConnectionStringFromEnvironmentVariables() =>
            _defaultMask.ReplaceIntegrationEventEnvironmentVariables(DatabaseEnvironmentVariables
                .GetIntegrationEventsEnvironmentVariables());

        private static string ReplaceEnvironmentVariables(this string connectionString, DatabaseEnvironmentVariables environmentVariables) => connectionString
            .Replace(DatabaseEnvironmentVariables.DbHostPlaceholder, environmentVariables.DbHost)
            .Replace(DatabaseEnvironmentVariables.DbNamePlaceholder, environmentVariables.DbName)
            .Replace(DatabaseEnvironmentVariables.SaPasswordPlaceholder, environmentVariables.SaPassword);
        
        private static string ReplaceIntegrationEventEnvironmentVariables(this string connectionString, DatabaseEnvironmentVariables environmentVariables) => connectionString
            .Replace(DatabaseEnvironmentVariables.DbHostPlaceholder, environmentVariables.DbHost)
            .Replace(DatabaseEnvironmentVariables.DbNamePlaceholder, environmentVariables.DbName)
            .Replace(DatabaseEnvironmentVariables.SaPasswordPlaceholder, environmentVariables.SaPassword);
    }
}   