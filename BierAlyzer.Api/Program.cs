using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BierAlyzer API",
        Version = "v1",
        Description = "RESTful API for BierAlyzer",
        Contact = new OpenApiContact
        {
            Name = "BierAlyzer Team",
            Email = "contact@bieralyzer.com"
        }
    });
    options.EnableAnnotations();
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Dependency Injection for services and repositories
builder.Services.AddScoped<BierAlyzer.Api.Services.IDrinkService, BierAlyzer.Api.Services.DrinkService>();
builder.Services.AddSingleton<BierAlyzer.Api.Repositories.IDrinkRepository, BierAlyzer.Api.Repositories.InMemoryDrinkRepository>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BierAlyzer.Api.Mappings.DrinkMappingProfile>());

// User services and repositories
builder.Services.AddScoped<BierAlyzer.Api.Services.IUserService, BierAlyzer.Api.Services.UserService>();
builder.Services.AddSingleton<BierAlyzer.Api.Repositories.IUserRepository, BierAlyzer.Api.Repositories.InMemoryUserRepository>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BierAlyzer.Api.Mappings.UserMappingProfile>());

// Global exception handling middleware registration placeholder
builder.Services.AddTransient<BierAlyzer.Api.Middleware.ExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BierAlyzer API v1");
        options.DocumentTitle = "BierAlyzer API Documentation";
        options.InjectStylesheet("/swagger-ui/custom.css");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Global exception handling middleware registration placeholder
app.UseMiddleware<BierAlyzer.Api.Middleware.ExceptionHandlingMiddleware>();

app.Run();

public partial class Program { }
