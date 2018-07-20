using Microsoft.AspNetCore.Http;
using React.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using IntranetApplication.Engines;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IntranetApplication.Models;
using IntranetApplication.Models.Authentication;
using IntranetApplication.Models.CarouselImages;
using IntranetApplication.Models.HtmlScrapping;
using IntranetApplication.Models.NewHire;
using IntranetApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.Webpack;

namespace IntranetApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();

            services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
            });
            /*========================================================================================================================================*/
            /*======================= Items for Authorization and Authentication =====================================================================*/
            /*========================================================================================================================================*/

           services.AddDbContext<ApplicationUserContext>(options =>
              options.UseSqlServer("Server = PLXLicensingDB ; Database = IntranetDB; User Id=test; Password = Penlink123;"));

            
            services.AddIdentity<ApplicationUser, IdentityRole>() // add applicationUser as Identity role
                .AddEntityFrameworkStores<ApplicationUserContext>()
                .AddDefaultTokenProviders(); // used to generate token for reset password and such
            
            
            services.AddMvc(config => // basically makes everything require authentication, set stuff to anonymous if you want it
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
          
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(14); // sets how long the user would stay logged in
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator")); // create a policy that can be used in view to show/not show things
            });

            /*========================================================================================================================================*/
            /*========================================================================================================================================*/
            /*========================================================================================================================================*/

            /*
             C:\Program Files\Microsoft SQL Server\140\Setup Bootstrap\Log\20180529_091835
               Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 2.0.3
                Add-Migration Initial
                Update-Database -context HtmlTargetContext
                Update-Database -context UpcomingEventsContext
             */
            
            services.AddDbContext<HtmlTargetContext>(options =>
                options.UseSqlServer("Server = PLXLicensingDB ; Database = IntranetDB; User Id=test; Password = Penlink123;"));

            services.AddDbContext<NewHireContext>(options =>
                options.UseSqlServer("Server = PLXLicensingDB ; Database = IntranetDB; User Id=test; Password = Penlink123;"));

            services.AddDbContext<CarouselImagesContext>(options =>
                options.UseSqlServer("Server = PLXLicensingDB ; Database = IntranetDB; User Id=test; Password = Penlink123;"));

    
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>(); // adds my own validation so that user's email address is penlink.om

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseAuthentication(); // this needs to come before the hangfire stuff

            // Hangfire Config 
            app.UseHangfireDashboard("/hangfire",new DashboardOptions()
            {
                Authorization =new [] {new HangfireAuthorization() }
            });
            
            app.UseHangfireServer();

            

            // host files by default by the wwwroot directory
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            StartJobs();
        }
        public void StartJobs()
        {
          ///  RecurringJob.AddOrUpdate<TransformEngine>(job => job.HtmlToJpegConverter(), Cron.MinuteInterval(5));
        }
    }
}
