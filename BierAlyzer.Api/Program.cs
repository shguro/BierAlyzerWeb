using System;
using System.Linq;
using BierAlyzer.Api.Models;
using BierAlyzer.Contracts.Model;
using BierAlyzer.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BierAlyzer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region EnsureAdmin

            var factory = new BierAlyzerContextFactory();
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var context = factory.CreateDbContext(null))
            {
                var defaultAdminMail = "bier@troogs.de";

                var adminUser = context.User.FirstOrDefault(u => u.Mail.ToLower() == defaultAdminMail);
                if (adminUser == null)
                {
                    var user = new User
                    {
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Enabled = true,
                        Hash = "B1B57C0699ED6120AA594127C84DB895",
                        Salt = "71BFDCDED04E94A8939E20A0DB8B174D",
                        Mail = defaultAdminMail,
                        Type = UserType.Admin,
                        Username = "Admin",
                        Origin = "Uni Siegen"
                    };

                    context.User.Add(user);
                }
                else
                {
                    adminUser.Type = UserType.Admin;
                }

                context.SaveChanges();
            }

            #endregion

            using (var host = CreateHostBuilder(args).Build())
            {
                var config = host.Services.GetService<IConfiguration>();

                try
                {
                    host.Run();
                }
                finally
                {
                    // TODO: Perform log
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates web host builder. </summary>
        /// <remarks>   Andre Beging, 17.11.2018. </remarks>
        /// <param name="args"> The arguments. </param>
        /// <returns>   The new web host builder. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5001");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
