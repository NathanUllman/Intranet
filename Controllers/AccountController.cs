using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Models;
using IntranetApplication.Models.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Interactions;

namespace IntranetApplication.Controllers
{

    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _Log;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _Log = loggerFactory.CreateLogger<AccountController>();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()

        {
            // Clear the existing external cookie and sign in to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            //await _signInManager.SignOutAsync();

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            using (_Log.BeginScope("User: {User}, Email: {modelEmail}", User.Identity.Name, model.Email)) // any logs within this using have this info now
            {
                try
                {
                    _Log.LogInformation("Login Started");
                    if (ModelState.IsValid)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                            model.RememberMe,
                            lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            _Log.LogInformation("User Login Successful");
                            return Redirect("/");
                        }
                        else
                        {
                            _Log.LogWarning("Login failed");
                            ModelState.AddModelError(string.Empty, "Login Failed");
                        }

                    }
                }
                catch (Exception e)
                {
                    _Log.LogCritical(e, "Login has thrown an exception");
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            //this function is for actually being able to go to the page
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            using (_Log.BeginScope("User: {User}, Email: {modelEmail}", User.Identity.Name, model.Email)) // any logs within this using have this info now
            {
                try
                {

                    if (ModelState.IsValid)
                    {
                        _Log.LogInformation("User Register Started");

                        var user = new ApplicationUser // create user
                        {
                            UserName = model.Email,
                            Email = model.Email
                        };
                        var result =
                            await _userManager.CreateAsync(user,
                                model.Password); // adds user to db using built in Manager


                        if (result.Succeeded)
                        {
                            _Log.LogInformation("User Register Successful");


                            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, Constants.DefaultRole);

                            if (roleResult.Succeeded)
                            {
                                _Log.LogInformation("User successfully added as default user role");
                            }
                            else
                            {
                                _Log.LogCritical("User failed being added to default role");
                            }

                            /* Items for email verification 
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                            await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                            */

                            await _signInManager.SignInAsync(user, isPersistent: false); // sign in user

                            return Redirect("/"); //RedirectToLocal(returnUrl);
                        }

                        _Log.LogWarning("User Register Failed, message: {errors}", result.Errors.ToString());
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty,
                                error.Description); // add errors to user so that they can be displayed to user
                        }
                    }

                }
                catch (Exception e)
                {
                    _Log.LogCritical(e,"Register user has thrown an error");
                }

                return View(model);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied(string returnUrl = null)
        {
            using (_Log.BeginScope("{User}", User.Identity.Name))
            {
                //Serilog.Log.Logger.ForContext("User",User,true).Information("Denying access to url {url}",returnUrl); fancy Logan code

                _Log.LogWarning("User: has been denied access to {returnUrl}. Big Brother is Watching", returnUrl);

                return View();
            }
        }
    }
}