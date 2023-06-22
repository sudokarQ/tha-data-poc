namespace Infrastructure.Seeders
{
    using Infrastructure.UnitOfWork;

    using Microsoft.AspNetCore.Identity;

    public class UserRolesSeeder
    {
        public const string AdminName = "Admin";

        public const string ScientistName = "Scientist";

        public static async Task Initialize(
            IUnitOfWork unitOfWork, 
            RoleManager<IdentityRole> roleManager)
        {
            if (await unitOfWork.GetRepository<IdentityRole>().AnyAsync())
            {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole(AdminName));
            await roleManager.CreateAsync(new IdentityRole(ScientistName));
        }
    }
}
