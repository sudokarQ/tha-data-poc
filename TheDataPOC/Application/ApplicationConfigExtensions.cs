namespace Application
{
    using Infrastructure.UnitOfWork;

    using Microsoft.Extensions.DependencyInjection;

    using Services;
    using Services.Interfaces;

    public static class ApplicationConfigExtensions
	{
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<ICrashService, CrashService>();
            
            return services;
        }
    }
}

