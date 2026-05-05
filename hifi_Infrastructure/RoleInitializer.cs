using Microsoft.AspNetCore.Identity;
using hifi_Infrastructure.Models;

namespace hifi_Infrastructure;

public class RoleInitializer
{
    public static async Task InitializeAsync(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync("Admin") == null)
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (await roleManager.FindByNameAsync("Customer") == null)
            await roleManager.CreateAsync(new IdentityRole("Customer"));

        string adminEmail = "admin@vuhaindustries.com";
        string adminPassword = "Admin_1234";

        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Адміністратор"
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}