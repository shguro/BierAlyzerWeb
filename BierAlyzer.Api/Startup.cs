using System.Text;
using AutoMapper;
using BierAlyzer.Api.Helper;
using BierAlyzer.Api.Models;
using BierAlyzer.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using WebApiContrib.Core.Formatter.Protobuf;

namespace BierAlyzer.Api
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Startup configuration class </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Startup
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="configuration">    The currently used configuration. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   The currently used configuration. </summary>
        /// <value> The configuration. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IConfiguration Configuration { get; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="services"> The services. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMemoryCache()
                .AddDbContext<BierAlyzerContext>(options => options.UseMySql(Configuration.GetConnectionString("Database")))
                .AddTransient<AuthService>()
                .AddTransient<EventService>()
                .AddTransient<UserService>()
                .AddTransient<ManagementService>();

            services.AddAutoMapper();

            services.AddMvc(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddXmlDataContractSerializerFormatters()
                .AddProtobufFormatters();
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BierAlyzer API", Version = "v1" });
                c.IncludeXmlComments("bin/BierAlyzerApi.xml");
            });

            // Setup authentication
            var jwtConfiguration = Configuration.GetSection("Jwt");
            var key = jwtConfiguration.GetValue<string>("Key");
            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfiguration.GetValue<string>("Issuer"),
                    ValidAudience = jwtConfiguration.GetValue<string>("Audience"),
                    LifetimeValidator = AuthenticationHelper.ValidateLifetime,
                    IssuerSigningKey = secKey,
                };
            });

            services.AddSingleton(Configuration);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request
        /// pipeline.
        /// </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="app">  The application. </param>
        /// <param name="env">  The environment. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
