namespace Application
{
    using Application.AutoMapping;

    using AutoMapper;

    using Microsoft.Extensions.DependencyInjection;

    using Services;
    using Services.Interfaces;

    public static class ApplicationConfigExtensions
	{
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper();

            return services.AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUploadService, UploadService>();

            services.AddScoped<ICrashService, CrashService>();

            services.AddScoped<ITrafficService, TrafficService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            
            services.AddScoped<IDocumentService, DocumentService>();

            
            return services;
        }

        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TrafficProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            return services.AddSingleton(mapper);
        }
    }
}

