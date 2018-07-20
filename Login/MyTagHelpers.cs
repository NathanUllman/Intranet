using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace IntranetApplication
{
    [HtmlTargetElement("td",Attributes = "user-roles")]
    public class MyTagHelpers : TagHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MyTagHelpers> _Log;

        public MyTagHelpers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _Log = loggerFactory.CreateLogger<MyTagHelpers>();
        }

        [HtmlAttributeName("user-roles")]
        public string userId { get; set; } // this basically grabs what user-roles is set equal to

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            _Log.LogInformation("Tag Helper 'user-roles' process started");
            IList<string> userRoles = new List<string>();
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    Id = userId
                };

                userRoles = await _userManager.GetRolesAsync(user);

                output.Content.SetContent(string.Join(", ", userRoles));
            }
            catch (Exception e)
            {
                _Log.LogWarning(e, "Failure in ProcessAsync() for tag helper 'user-roles'");
            }
        }
    }
}
