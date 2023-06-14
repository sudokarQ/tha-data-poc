﻿namespace API
{
    using Application;

    using Infrastructure;

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

            services
                .AddApplication()
                .AddInfrastructure(connection, configuration);

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
                .UseSwagger()
                .UseSwaggerUI()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}