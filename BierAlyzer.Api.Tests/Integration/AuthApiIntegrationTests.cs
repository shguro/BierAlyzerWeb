using BierAlyzer.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace BierAlyzer.Api.Tests.Integration
{
    public class AuthApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public AuthApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenValid()
        {
            var client = _factory.CreateClient();
            var login = new LoginRequestDto { Username = "admin", Password = "admin" };
            var response = await client.PostAsJsonAsync("/api/auth/login", login);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalid()
        {
            var client = _factory.CreateClient();
            var login = new LoginRequestDto { Username = "admin", Password = "wrong" };
            var response = await client.PostAsJsonAsync("/api/auth/login", login);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
