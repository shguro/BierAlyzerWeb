using BierAlyzer.Api.Controllers;
using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BierAlyzer.Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _serviceMock = new();
        private readonly Mock<ILogger<AuthController>> _loggerMock = new();
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenValid()
        {
            var login = new LoginRequestDto { Username = "admin", Password = "admin" };
            var response = new LoginResponseDto { UserId = 1, Username = "admin", DisplayName = "Administrator", Token = "demo-token-1" };
            _serviceMock.Setup(s => s.LoginAsync(login)).ReturnsAsync(response);
            var result = await _controller.Login(login);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.UserId, ((LoginResponseDto)ok.Value!).UserId);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalid()
        {
            var login = new LoginRequestDto { Username = "admin", Password = "wrong" };
            _serviceMock.Setup(s => s.LoginAsync(login)).ReturnsAsync((LoginResponseDto?)null);
            var result = await _controller.Login(login);
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
