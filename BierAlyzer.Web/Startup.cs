using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUglify.Css;
using NUglify.JavaScript;

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
            #region Bundle Configurations

            var javascriptBundleSettings = new CodeSettings
            {
                PreserveImportantComments = false,
                MinifyCode = false,
                IgnoreAllErrors = true
            };

            var cssSettings = new CssSettings
            {
                MinifyExpressions = true,
                CommentMode = CssComment.None,
            };

            #endregion

            services.AddControllersWithViews();
            services.AddWebOptimizer(pipeline =>
            {
                #region Bundles

                // Global CSS
                pipeline.AddCssBundle(
                    "/css/bieralyzer.min.css",
                    cssSettings,
                    "/css/global/bootstrap-4.1.0.min.css",
                    "/css/global/bootstrap-material-design.min.css",
                    "/css/global/fontawesome-5.0.11.all.css",
                    "/css/global/materialdesign-custom.css",
                    "/css/global/site.css");

                // Typeahead CSS
                pipeline.AddCssBundle(
                    "/css/typeahead.min.css",
                    cssSettings,
                    "/css/typeahead.css");

                // Global JS
                pipeline.AddJavaScriptBundle(
                    "/js/bieralyzer.min.js",
                    javascriptBundleSettings,
                    "/js/global/jquery-3.3.1.slim.min.js",
                    "/js/global/popper-1.14.3.min.js",
                    "/js/global/bootstrap-4.1.0.min.js",
                    "/js/global/bootstrap-material-design.min.js",
                    "/js/global/site.js");

                // Typeahead JS
                pipeline.AddJavaScriptBundle(
                    "/js/typeahead.min.js",
                    javascriptBundleSettings,
                    "/js/typeahead.js",
                    "/js/typeahead-matcher.js");

                #endregion
            });
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
            #region Exception Configuration

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            #endregion

            app.UseStaticFiles();
            app.UseSession();

            app.UseWebOptimizer();

            app.UseRouting();

            app.UseEndpoints(routes =>
            {
                #region Routes

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

                #endregion
            });
        }
    }
}