namespace API
{
    public static class EnviromentVaribleExtensions
    {
        public static string? GetDbConnectionString(this IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                var dbUrl = Environment.GetEnvironmentVariable("DB_URL");
                var dbName = Environment.GetEnvironmentVariable("DB_NAME");
                var dbUser = Environment.GetEnvironmentVariable("DB_USER");
                var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

                var connection = $"Data Source={dbUrl};Initial Catalog={dbName};User ID={dbUser};Password={dbPassword};Encrypt=False;";

                return connection;
            }

            return configuration["TheDataPOCDb"];
        }
    }
}
