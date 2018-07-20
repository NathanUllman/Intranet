using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Data;
using IntranetApplication.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace IntranetApplication.Services
{
    public class CustomUserValidator : UserValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user) // basically called when user is trying to register new account
        {
            IdentityResult result = await base.ValidateAsync(manager, user); // first uses the base validation

            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList(); //checks errors for base validation

            if (!user.Email.ToLower().EndsWith("@penlink.com")) //then do my own validation on top of base validation
            {
                errors.Add(new IdentityError
                {
                    Code = "EmailDomainError",
                    Description = "Only penlink.com email addresses are allowed :)"
                });
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()); 

        }

       
    }
    
    
}
