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
    public class DrinksControllerTests
    {
        private readonly Mock<IDrinkService> _serviceMock = new();
        private readonly Mock<ILogger<DrinksController>> _loggerMock = new();
        private readonly DrinksController _controller;

        public DrinksControllerTests()
        {
            _controller = new DrinksController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfDrinks()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DrinkDto> { new DrinkDto { Id = 1, Name = "Test", Abv = 5.0 } });
            var result = await _controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<DrinkDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotExists()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DrinkDto?)null);
            var result = await _controller.GetById(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtAction()
        {
            var dto = new DrinkDto { Id = 1, Name = "Test", Abv = 5.0 };
            _serviceMock.Setup(s => s.AddAsync(dto)).ReturnsAsync(dto);
            var result = await _controller.Add(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(dto.Id, ((DrinkDto)created.Value!).Id);
        }
    }
}
