namespace Infrastructure
{
    using Infrastructure.Stores;
    using Database;
    using Domain.Models;
    using Microsoft.AspNetCore.Identity;
    using UnitOfWork;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.UnitOfWork;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString,
            IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddTransient<IUserStore<User>, UserStore>();   
            services.AddTransient<IRoleStore<Role>, RoleStore>();
            
            services
                .AddDbContext<ApplicationContext>(opt =>
                {
                    opt.UseSqlServer(connectionString, opt =>
                    {
                        opt.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
                    });
                    opt.EnableSensitiveDataLogging();
                });

            var builder = services.AddIdentityCore<User>();

            builder.AddRoles<Role>();
            builder.AddUserManager<UserManager<User>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.ConfigureApplicationCookie(configure => {
                configure.AccessDeniedPath = "/api/AccessDenied";
            });

            var serviceProvider = services.BuildServiceProvider();

            return services;
        }            
    }
}
