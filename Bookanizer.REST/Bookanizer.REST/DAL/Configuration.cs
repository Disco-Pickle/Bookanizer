namespace Bookanizer.REST.DAL
{
    public class Configuration
    {
        #region Properties
        public static string PostgresConnectionString => GetPostgresConnectionString();
        #endregion

        #region Methods
        private static string GetPostgresConnectionString()
        {
            string host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? throw new InvalidOperationException("POSTGRES_HOST environment variable is not set");
            string port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? throw new InvalidOperationException("POSTGRES_PORT environment variable is not set");
            string database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? throw new InvalidOperationException("POSTGRES_DATABASE environment variable is not set");
            string username = Environment.GetEnvironmentVariable("POSTGRES_USERNAME") ?? throw new InvalidOperationException("POSTGRES_USERNAME environment variable is not set");
            string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? throw new InvalidOperationException("POSTGRES_PASSWORD environment variable is not set");

            return $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        }
        #endregion
    }
}
