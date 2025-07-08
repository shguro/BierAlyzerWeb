using BierAlyzer.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace BierAlyzer.Api.Tests.Integration
{
    public class DrinksApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public DrinksApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllDrinks_ReturnsSuccess()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/drinks");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddDrink_ReturnsCreated()
        {
            var client = _factory.CreateClient();
            var dto = new DrinkDto { Name = "Test", Abv = 4.2 };
            var response = await client.PostAsJsonAsync("/api/drinks", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
