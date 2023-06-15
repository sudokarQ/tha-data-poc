namespace API
{
    using System.Text;
    
    using Application;

    using Infrastructure;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = configuration.GetDbConnectionString(environment);

            services.AddAutoMapper(typeof(Startup));

            var key = Encoding.ASCII.GetBytes("Crew24TheDataPOCSecretKey");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            

            services.AddInfrastructure(connection, configuration);
            services.AddApplication();
            services
                .AddSwaggerGen()
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseSwagger()
                .UseSwaggerUI()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}