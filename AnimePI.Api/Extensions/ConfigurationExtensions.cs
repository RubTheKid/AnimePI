namespace AnimePI.Api.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConnectionStringForEnvironment(this IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }


        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
        {
            connectionString = ReplaceHostForDocker(connectionString);
            Console.WriteLine("Running in Docker container, using connection string with sqlserver");
        }

        return connectionString;
    }

    private static string ReplaceHostForDocker(string connectionString)
    {
        return connectionString
            .Replace("localhost", "sqlserver")
            .Replace("host.docker.internal", "sqlserver");
    }
}

