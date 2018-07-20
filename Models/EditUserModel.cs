using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace IntranetApplication.Models
{
    public class EditUserModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> UserRoles { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
    }
}
