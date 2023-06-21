namespace Infrastructure.Database
{
    using Domain.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

	public class ApplicationContext : IdentityDbContext<User>
    {
        public virtual DbSet<Crash> Crashes { get; set; }

        public virtual DbSet<Pedestrian> Pedestrians { get; set; }

        public virtual DbSet<Traffic> Traffics { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}

