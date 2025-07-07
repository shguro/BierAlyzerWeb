using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BierAlyzerWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(72);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseEndpoints(routes =>
            {
                // AccountController
                routes.MapControllerRoute("Login", "login", new { controller = "Account", action = "Login" });
                routes.MapControllerRoute("Logout", "logout", new { controller = "Account", action = "Logout" });
                routes.MapControllerRoute("SignUp", "signup", new { controller = "Account", action = "SignUp" });

                // HomeController
                routes.MapControllerRoute("Events", "events", new { controller = "Home", action = "Events" });
                routes.MapControllerRoute("Event", "event", new { controller = "Home", action = "Event" });
                routes.MapControllerRoute("UserProfile", "profile", new { controller = "Home", action = "UserProfile" });
                routes.MapControllerRoute("JoinPublicEvent", "joinpublic", new { controller = "Home", action = "JoinPublicEvent" });
                routes.MapControllerRoute("BookDrink", "book", new { controller = "Home", action = "BookDrink" });
                routes.MapControllerRoute("LeaveEvent", "leave", new { controller = "Home", action = "LeaveEvent" });
                routes.MapControllerRoute("UserEvents", "userevents", new { controller = "Home", action = "UserEvents" });

                // Archive Controller
                routes.MapControllerRoute("Archive", "archive", new { controller = "Archive", action = "Archive" });

                // ManagementController
                routes.MapControllerRoute("ManageEvents", "manageevents", new { controller = "Management", action = "Events" });
                routes.MapControllerRoute("ManageEvent", "manageevent", new { controller = "Management", action = "Event" });
                routes.MapControllerRoute("ManageUsers", "manageusers", new { controller = "Management", action = "Users" });
                routes.MapControllerRoute("ManageDrinks", "managedrinks", new { controller = "Management", action = "Drinks" });
                routes.MapControllerRoute("ManageDrink", "managedrink", new { controller = "Management", action = "Drink" });
                routes.MapControllerRoute("SetEventType", "seteventtype", new { controller = "Management", action = "SetEventType" });
                routes.MapControllerRoute("SetEventStatus", "seteventstatus", new { controller = "Management", action = "SetEventStatus" });
                routes.MapControllerRoute("ToggleDrinkVisibility", "drinkvisibility", new { controller = "Management", action = "ToggleDrinkVisibility" });
                routes.MapControllerRoute("ToggleUserEnabled", "UserEnabled", new { controller = "Management", action = "ToggleUserEnabled" });

                // PublicController
                routes.MapControllerRoute("Error", "error", new { controller = "Public", action = "Error" });
                routes.MapControllerRoute("Impressum", "impressum", new { controller = "Public", action = "Impressum" });
                routes.MapControllerRoute("Privacy", "privacy", new { controller = "Public", action = "Privacy" });

                // Default
                routes.MapControllerRoute("default", "{controller}/{action}", new { controller = "Home", action = "Events" });
                routes.MapControllerRoute("fallback", "{*url}", new { controller = "Home", action = "Events" });
            });
        }
    }
}