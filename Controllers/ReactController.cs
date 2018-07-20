using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IntranetApplication.Engines;
using IntranetApplication.Models;
using IntranetApplication.Models.Authentication;
using IntranetApplication.Models.CarouselImages;
using IntranetApplication.Models.NewHire;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IntranetApplication.Models.DashboardItem;

namespace IntranetApplication.Controllers
{
    [Route("[controller]/[action]")]
    public class ReactController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ReactController> _Log;
        private readonly NewHireContext _newHireContext;
        private readonly CarouselImagesContext _carouselImagesContext;
        
        public ReactController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory,
            NewHireContext newHireContext,
            CarouselImagesContext carouselImagesContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _Log = loggerFactory.CreateLogger<ReactController>();
            _newHireContext = newHireContext;
            _carouselImagesContext = carouselImagesContext;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> files, string location)
        {
            long size = files.Sum(f => f.Length);

            var domain = Request.GetUri().ToString().Replace("/React/UploadImage", ""); // used to get the domain of the website
            location = location.Replace(domain, "wwwroot");
            var filePath = location;

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserRoles() // called by our react components to determine what user should see
        {
            _Log.LogInformation("User Role Retrieval Started");
            try
            {
                ApplicationUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name); // name based on email, so name is unique
                var roles = await _userManager.GetRolesAsync(currentUser);
                return Content(JsonConvert.SerializeObject(roles));
            }
            catch (Exception e)
            {
                _Log.LogWarning(e, "Failure in User Role Retrieval");
                return Content(JsonConvert.SerializeObject(""));
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetNewHireInfo()
        {
            _Log.LogInformation("Get New Hire Info started");
            try
            {
                var response = _newHireContext.NewHires.ToList();
                return Content(JsonConvert.SerializeObject(response));

            }
            catch (Exception e)
            {
                _Log.LogWarning(e,"Failure in Get New Hire Info");
            }
            return Content(JsonConvert.SerializeObject(""));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCarouselImages()
        {
            _Log.LogInformation("Get CarouselImages started");
            try
            {
                var response = _carouselImagesContext.CarouselImages.ToList();
                return Content(JsonConvert.SerializeObject(response));
            }
            catch(Exception e)
            {
                _Log.LogWarning(e, "Failure in Get New Hire Info");
            }
            return Content(JsonConvert.SerializeObject(""));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDashboards()
        {
            DbAccessor accessor = new DbAccessor();
            List<Dashboard> items = accessor.GetActiveDashboards();       
            return Content(JsonConvert.SerializeObject(items.OrderBy(x => x.SortOrder).ToList()));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetItemsForDashboard()
        {

            DbAccessor accessor = new DbAccessor();
            List<DashboardItem> items = accessor.GetActiveDashboardItems();
            return Content(JsonConvert.SerializeObject(items.OrderBy(x => x.SortOrder).ToList()));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpdateDashboards([FromBody] List<Dashboard> dashboardsToUpdate)
        {
      
            DbAccessor accessor = new DbAccessor();
            foreach (var dash in dashboardsToUpdate)
            {
                accessor.UpdateDashboard(dash);
            }
            return Content("");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpdateDashboardItems([FromBody] List<DashboardItem> dashboardItemsToUpdate)
        {
               
                DbAccessor accessor = new DbAccessor();
                foreach (var dashItem in dashboardItemsToUpdate)
                {
                   accessor.UpdateDashItem(dashItem);
                }
                    
            return Content("");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> DeleteDashboardItem(int DashboardItemId)
        {

            DbAccessor accessor = new DbAccessor();

            accessor.DeleteDashItem(DashboardItemId);

            return Redirect("/AdminTools/DashManager");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> DeleteDashboard(int DashboardId)
        {

            DbAccessor accessor = new DbAccessor();

            accessor.DeleteDashboard(DashboardId);

            return Redirect("/AdminTools/DashManager");
        }

    }
}
