using ElKood.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace elkood_Task.SeedExtension
{
    public static class SeedData
    {
        public static async Task Initialize(this IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Owner", "Guest" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var defaultUser = new ApplicationUser
            {
                UserName = "Owner",
                Email = "Owner@elkoodtask.com",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(defaultUser.Email) == null)
            {
                var result = await userManager.CreateAsync(defaultUser, "Aa@123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Owner");
                }
            }
        }
    }
}
