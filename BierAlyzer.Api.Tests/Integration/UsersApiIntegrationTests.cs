using BierAlyzer.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace BierAlyzer.Api.Tests.Integration
{
    public class UsersApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public UsersApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllUsers_ReturnsSuccess()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddUser_ReturnsCreated()
        {
            var client = _factory.CreateClient();
            var dto = new UserDto { Username = "newuser", DisplayName = "New User" };
            var response = await client.PostAsJsonAsync("/api/users", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
