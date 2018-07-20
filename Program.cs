using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Data;
using IntranetApplication.Models.Authentication;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;


namespace IntranetApplication
{
    public class Program
    {
        private static Logger _Logger;

        public static void Main(string[] args)
        {
            _Logger = GetLogger(); // so we can use logger for Program.cs
            _Logger.Information("Application Started");

            Log.Logger = GetLogger(); // sets configuration for Ilogger which is used in controllers and such

/*===================================================================================================*/
/*====================== Seeding Database ===========================================================*/
/*===================================================================================================*/
            try
            {
                _Logger.Verbose("Building web host");

                var host = BuildWebHost(args);

                using (var scope = host.Services.CreateScope())
                {

                    var services = scope.ServiceProvider;

                    // var config = host.Services.GetRequiredService<IConfiguration>();             
                    var testUserPw = "Super123@"; // config["SeedUserPW"]; // dotnet user-secrets set SeedUserPW <pw>

                    try
                    {
                        SeedData.Initialize(services, testUserPw)
                            .Wait(); //add an initial admin account to the db on startup, also add Editor role
                    }
                    catch (Exception ex)
                    {
                        _Logger.Warning(ex, "An error occurred while seeding the database.");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                _Logger.Fatal(ex, "Failed to start applicaition");
                Log.CloseAndFlush();
            }
            finally
            {
                _Logger.Information("Web host has stopped");
                Log.CloseAndFlush();
            }

/*=================================================================================================== */
/*=================================================================================================== */
/*=================================================================================================== */
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog() // sets Serilog to be used as Ilogger
                .Build();

        private static Logger GetLogger() => new LoggerConfiguration()
            .MinimumLevel.Verbose()
            //.Enrich.FromLogContext()
            //.Enrich.WithMachineName()
            //.Enrich.WithExceptionStackTraceHash()
            //.Enrich.WithThreadId()
            //.Enrich.WithUserName()
            //.Enrich.WithProperty("Context", "Web")
            //.Enrich.With(new StackTraceEnricher("PLX.AutoloadMonitor"))
            //.Enrich.With<ExceptionDataEnricher>()
            //.WriteTo.Console()
            //.WriteTo.Seq("http://localhost:5341") //https://getseq.net/Download
            //.WriteTo.File("app.log")
            .CreateLogger();
    }

}
