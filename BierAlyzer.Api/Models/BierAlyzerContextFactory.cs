using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BierAlyzer.Api.Models
{
    /// <inheritdoc />
    public class BierAlyzerContextFactory : IDesignTimeDbContextFactory<BierAlyzerContext>
    {
        #region CreateDbContext

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates database context. </summary>
        /// <remarks>   Andre Beging, 29.04.2018. </remarks>
        /// <param name="args"> The arguments. </param>
        /// <returns>   The new database context. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public BierAlyzerContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());

            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
                configurationBuilder.AddJsonFile($"appsettings.Development.json", optional: true);
            else
                configurationBuilder.AddJsonFile($"appsettings.json", optional: true);
            

            var configuration = configurationBuilder.Build();

            var builder = new DbContextOptionsBuilder<BierAlyzerContext>();

            var connectionString = configuration.GetConnectionString("Database");

            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            //AB20181108 In case some magical creature makes sqlite foreigns key available
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
            //    builder.UseSqlite(connectionString);
            //else
            //    builder.UseMySql(connectionString);

            return new BierAlyzerContext(builder.Options);
        }

        #endregion
    }
}