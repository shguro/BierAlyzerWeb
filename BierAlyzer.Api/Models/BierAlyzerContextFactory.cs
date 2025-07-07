using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

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

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == EnvironmentName.Development)
                configurationBuilder.AddJsonFile($"appsettings.Development.json", optional: true);
            else
                configurationBuilder.AddJsonFile($"appsettings.json", optional: true);

            var configuration = configurationBuilder.Build();
            var builder = new DbContextOptionsBuilder<BierAlyzerContext>();
            var connectionString = configuration.GetConnectionString("Database");
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            return new BierAlyzerContext(builder.Options);
        }

        #endregion
    }
}