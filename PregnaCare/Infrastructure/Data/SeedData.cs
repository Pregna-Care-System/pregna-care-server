using Microsoft.AspNetCore.Identity;

namespace PregnaCare.Infrastructure.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Seed roles
            var roleNames = new List<string> { "Admin", "Member", "Guest" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        ConcurrencyStamp = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                    });
                }
            }

            // Seed admin account
            var adminEmail = "pregnacareadmin@gmail.com";
            var adminPassword = "Admin1234@!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    ConcurrencyStamp = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                };

                await userManager.CreateAsync(admin, adminPassword);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
