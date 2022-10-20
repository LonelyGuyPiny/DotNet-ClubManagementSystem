using ClubManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Data
{
    public static class DbInitializer
    {
        private const string AdminEmail = "admin@admin.com";
        private const string AdminPassword = "Password11!";

        public static async Task ApplyMigrationsAndSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    await dbContext.Database.MigrateAsync();

                    if (!await roleManager.RoleExistsAsync(Roles.Admin))
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin });
                    }

                    await CreateAdminUser(userManager);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static async Task CreateAdminUser(UserManager<User> userManager)
        {
            var admin = await userManager.FindByEmailAsync(AdminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    FirstName = "Almighty",
                    LastName = "Admin"
                };
                await userManager.CreateAsync(admin, AdminPassword);
            }
            if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
                await userManager.AddToRoleAsync(admin, Roles.Admin);
        }
    }
}
