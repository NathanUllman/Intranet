using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Engines;
using IntranetApplication.Models;
using IntranetApplication.Models.Authentication;
using IntranetApplication.Models.DashboardItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntranetApplication.Controllers
{
   // [Authorize(Roles = "Administrator")]
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class AdminToolsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminToolsController> _Log;

        public AdminToolsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _Log = loggerFactory.CreateLogger<AdminToolsController>();
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {

            return View(_userManager.Users.OrderBy(x => x.UserName));
        }
        [HttpGet]
        public async Task<IActionResult> DashManager()
        {
            return View();
        }

        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return RedirectToAction("UserList");
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            EditUserModel model = new EditUserModel
            {
                User = user,
                UserRoles = roles,
                AllRoles = allRoles
            };
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string roleName, string userId)
        {
            using (_Log.BeginScope("User: {User}, roleName: {roleName}, userId: {userId}", User.Identity.Name, roleName, userId)) // any logs within this using have this info now
            {
                try
                {
                    _Log.LogInformation("User Add to Role Started");
                    var user = await _userManager.FindByIdAsync(userId);

                    var addResult = await _userManager.AddToRoleAsync(user, roleName);

                    if (addResult.Succeeded)
                    {
                        _Log.LogInformation("User Successfully added to Role");
                        if (await _userManager.IsInRoleAsync(user, Constants.DefaultRole))
                        {
                            await _userManager.RemoveFromRoleAsync(user, Constants.DefaultRole); // user is no longer default, so remove default role         
                        }
                              
                    }
                    else
                    {
                        _Log.LogWarning("User failed being added to role, errors: {result}", addResult.Errors.ToString());
                    }
                }
                catch (Exception e)
                {
                    _Log.LogCritical(e,"Error thrown for adding user to role");
                }

                return RedirectToAction("EditUser", new {id = userId});
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(string roleName, string userId)
        {
            using (_Log.BeginScope("User: {User}, roleName: {roleName}, userId: {userId}", User.Identity.Name, roleName,userId)) // any logs within this using have this info now
            {
                try
                {
                    _Log.LogInformation("User Remove from Role Added");
                    var user = await _userManager.FindByIdAsync(userId);

                    var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                    if (result.Succeeded && (await _userManager.GetRolesAsync(user)).Count == 0
                    ) // user needs to be added to default role since they are not in any other role
                    {
                        await _userManager.AddToRoleAsync(user, Constants.DefaultRole);
                    }
                    else
                    {
                        _Log.LogCritical("Faled Removing User from user");
                    }
                }
                catch (Exception e)
                {
                    _Log.LogCritical(e,"Error thrown for removing a user from role");
                }
            }

            return RedirectToAction("EditUser", new { id = userId });
        }



        

        /*
        [HttpGet]
        public async Task<IActionResult> AddDashboardItem(string DashboardId)
        {
            ViewData["ImageSrc"] = "/images/preview.png";
            int  Id = Int32.Parse(DashboardId); // todo use tryparse to check if we can actually

            return View(new DashboardItem{DashboardID = Id });
        }
        */


        [HttpPost]
        public async Task<IActionResult> TestImage(DashboardItem newDashboardItem)
        {
         TransformEngine yeah = new TransformEngine();
         string location =  yeah.ConvertOneTEMP(newDashboardItem.LogonUser, newDashboardItem.LogonPwd, newDashboardItem.SourceURL,
                "#widgets-container");
            ViewData["ImageSrc"] = location;
            return View("AddDashboardItem", newDashboardItem);
        }

        [HttpGet]
        public async Task<IActionResult> EditDashboardItem(string DashboardItemId)
        {
            return View();
        }
    }
}