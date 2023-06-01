namespace Infrastructure.Database
{
    using Domain;
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
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(options => {
                options.Property(p => p.UserName)
                    .HasMaxLength(User.UserNameMaxLength)
                    .IsRequired();
                options.Property(p => p.NormalizedUserName)
                    .IsRequired();
                options.Property(p => p.PasswordHash)
                    .IsRequired();
                options.Property(p => p.Email)
                    .HasMaxLength(User.UserEmailMaxLength)
                    .IsRequired();
            });

            builder.Entity<Role>(options => {
                options.Property(p => p.RoleName)
                    .IsRequired();
                options.Property(p => p.NormalizedRoleName)
                    .IsRequired();

                options.HasData(
                    new Role {
                        Id = 1,
                        RoleName = RoleNames.Admin,
                        NormalizedRoleName = RoleNames.Admin.ToUpper()
                    },
                    new Role {
                        Id = 2,
                        RoleName = RoleNames.Scientist,
                        NormalizedRoleName = RoleNames.Scientist.ToUpper()
                    },
                    new Role {
                        Id = 3,
                        RoleName = RoleNames.User,
                        NormalizedRoleName = RoleNames.User.ToUpper()
                    });
            });

            builder.Entity<UserRole>(options => {
                options.HasKey(ck => new { ck.UserId, ck.RoleId});

                options.HasOne(ur => ur.User)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                options.HasOne(ur => ur.Role)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            });
        }
    }
}

