using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DemoEFCoreConsole.Configuration
{
    public static class ConfigurationFactory
    {
        const string _environmentVariable = "MY_ENVIRONMENT";
        static public IConfigurationRoot Create()
        {
            var environmentName = Environment.GetEnvironmentVariable(_environmentVariable);

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                            .AddEnvironmentVariables();

            return builder.Build();
        }

        public static DatabaseSettings GetDatabaseSettings(this IConfigurationRoot configuration)
        {
            var databaseSettings = new DatabaseSettings();
            configuration.GetSection("Database")?.Bind(databaseSettings);
            return databaseSettings;
        }
    }
}