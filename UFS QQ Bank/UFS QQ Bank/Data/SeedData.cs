using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public class SeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Ufs@123$";
        private const string adminEmail = "admin@ufs.ac.za";
        private const string adminRole = "Admin";
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            AppEntityDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppEntityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            //AppIdentityDbContext context = app.ApplicationServices
            //    .CreateScope().ServiceProvider
            //    .GetRequiredService<AppIdentityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<User> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<User>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            if (await userManager.FindByNameAsync(adminUser) == null)
            {
                if (await roleManager.FindByNameAsync(adminRole) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                }

                User user = new()
                {
                    UserName = adminUser,
                    Email = adminEmail,
                    FirstName = "AdminFirstName",  // Ensure FirstName is not null
                    LastName = "AdminLastName"
                };
                user.MobilePassword = adminPassword.ToString();

                IdentityResult result = await userManager.CreateAsync(user, adminPassword);
                

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }

            }
            context.SaveChanges();
        }
    }
}
