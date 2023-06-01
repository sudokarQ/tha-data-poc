namespace Infrastructure.Database
{
    using Domain.Models;

	using Microsoft.EntityFrameworkCore;

	public class ApplicationContext : DbContext
    {
        public virtual DbSet<Crash> Crashes { get; set; }

        public virtual DbSet<Pedestrian> Pedestrians { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Traffic> Traffics { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

