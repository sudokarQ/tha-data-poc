namespace Infrastructure
{
    using System.Reflection;

    using Database;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.SqlServer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using Repositories;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString,
            IConfiguration configuration)
        {
            services
                .AddDbContext<ApplicationContext>(opt =>
                {
                    opt.UseSqlServer(connectionString, opt =>
                    {
                        opt.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
                    });
                    opt.EnableSensitiveDataLogging();
                });
                      
            var serviceProvider = services.BuildServiceProvider();

            return services;
        }            
    }
}
