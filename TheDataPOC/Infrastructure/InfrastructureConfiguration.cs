namespace Infrastructure
{
    using Database;

    using UnitOfWork;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString,
            IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

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
