using BierAlyzer.Api.Controllers;
using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BierAlyzer.Api.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _serviceMock = new();
        private readonly Mock<ILogger<UsersController>> _loggerMock = new();
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _controller = new UsersController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfUsers()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<UserDto> { new UserDto { Id = 1, Username = "test", DisplayName = "Test" } });
            var result = await _controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotExists()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((UserDto?)null);
            var result = await _controller.GetById(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtAction()
        {
            var dto = new UserDto { Id = 1, Username = "test", DisplayName = "Test" };
            _serviceMock.Setup(s => s.AddAsync(dto)).ReturnsAsync(dto);
            var result = await _controller.Add(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(dto.Id, ((UserDto)created.Value!).Id);
        }
    }
}
