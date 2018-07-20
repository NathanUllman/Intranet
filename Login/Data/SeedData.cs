
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Controllers;
using IntranetApplication.Data;
using IntranetApplication.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Logging;

// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -udl -outDir Pages\Contacts --referenceScriptLibraries

namespace IntranetApplication.Data
{
    //todo: when a user is deleted in database, their cookie still allows them to log in and such
    public static class SeedData
    {

        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationUserContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationUserContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@penlink.com"); // creates user if one is not already created

                await EnsureRole(serviceProvider, adminID, Constants.AdminRole); // checks that newly created user has specifed role (admin)

                await AddRole(serviceProvider, Constants.DefaultRole);
                await AddRole(serviceProvider, Constants.EditorRole);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>(); // get user manager

            var user = await userManager.FindByNameAsync(UserName); // tried to find user in db
            if (user == null) // if user does not exist
            {
                user = new ApplicationUser { UserName = UserName, Email = UserName }; // create new user
                var result = await userManager.CreateAsync(user, testUserPw); // save user to db

            }

            return user.Id; // return Id we can use to find user in db
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
            string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>(); // get user manager

            if (!await roleManager.RoleExistsAsync(role)) // see if our role exists (adminRole)
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role)); // if not, create new
            }        

            var user = await userManager.FindByIdAsync(uid); // find user

            if (!await userManager.IsInRoleAsync(user, role)) //todo: figure out if this could go in a spot to stop more unneccessary processes 
            {
                IR = await userManager.AddToRoleAsync(user, role); // give user role (admin)
            }

            return IR;
        }

        private static async Task<IdentityResult> AddRole(IServiceProvider serviceProvider, string role)
        {
            IdentityResult result = null;

            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role)) 
            {
                 result = await roleManager.CreateAsync(new IdentityRole(role)); 
            }

            return result;
        }
    }
}