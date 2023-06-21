namespace Application
{
    using System.Text;

    using AutoMapper;
    
    using AutoMapping;
    
    using Domain.Models;
    
    using Infrastructure.Database;
    
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    
    using Services;
    using Services.Interfaces;
    
    using Sieve.Services;

    public static class ApplicationConfigExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication();

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>();

            services.ConfigureJWT(configuration);

            services.AddAutoMapper();

            services.AddScoped<ISieveProcessor, SieveProcessor>();

            services.AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUploadService, UploadService>();

            services.AddScoped<ICrashService, CrashService>();

            services.AddScoped<ITrafficService, TrafficService>();

            services.AddScoped<IPedestrianService, PedestrianService>();

            services.AddScoped<IDocumentService, DocumentService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserService, UserService>();

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

        private static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["secret"];

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    };
                    o.Audience = "TheDataPOC.API";
                });
        }
    }
}

