using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebPulse_WebManager.Models;
using WebPulse_WebManager.Constants;

namespace WebPulse_WebManager.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndGlobalAdmin(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.SystemOwner.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.GlobalAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));


            #region SystemOwner Setting

            var user = new ApplicationUser()
            {
                UserName = "thomasgrant0801@gmail.com",
                Email = "thomasgrant0801@gmail.com",
                EmailConfirmed = true,
            };

            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if(userInDb == null)
            {
                await userManager.CreateAsync(user, "Password123*");
                await userManager.AddToRoleAsync(user, Roles.SystemOwner.ToString());
            }


            #endregion
        }

    }
}
