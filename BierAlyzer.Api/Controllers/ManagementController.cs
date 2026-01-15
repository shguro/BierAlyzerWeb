using System;
using System.Linq;
using BierAlyzer.Api.Services;
using BierAlyzer.Contracts.Communication.Event;
using BierAlyzer.Contracts.Communication.Management.Request;
using BierAlyzer.Contracts.Communication.Management.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BierAlyzer.Api.Controllers
{
    [Route("api/management")]
    [Authorize]
    public class ManagementController : Controller
    {
        private readonly ManagementService _managementService;

        public ManagementController(ManagementService managementService)
        {
            _managementService = managementService;
        }

        [HttpGet("users")]
        [SwaggerResponse(200, typeof(UsersResponse))]
        [SwaggerResponse(400, typeof(UsersResponse))]
        public IActionResult GetUsers()
        {
            var response = _managementService.GetUsers(HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("user/{id}")]
        [SwaggerResponse(200, typeof(UsersResponse))]
        [SwaggerResponse(400, typeof(UsersResponse))]
        public IActionResult GetUser(Guid id)
        {
            var response = _managementService.GetUser(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("user")]
        [SwaggerResponse(200, typeof(UsersResponse))]
        [SwaggerResponse(400, typeof(UsersResponse))]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            var response = _managementService.UpdateUser(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("events")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult GetEvents()
        {
            var response = _managementService.GetAllEvents(HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("event/{id}")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult DeleteEvent(Guid id)
        {
            var response = _managementService.DeleteEvent(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("drinks")]
        [SwaggerResponse(200, typeof(DrinkResponse))]
        [SwaggerResponse(400, typeof(DrinkResponse))]
        public IActionResult GetDrinks()
        {
            var response = _managementService.GetDrinks(HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("drink")]
        [SwaggerResponse(200, typeof(DrinkResponse))]
        [SwaggerResponse(400, typeof(DrinkResponse))]
        public IActionResult CreateDrink([FromBody] CreateDrinkRequest request)
        {
            var response = _managementService.CreateDrink(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("drink")]
        [SwaggerResponse(200, typeof(DrinkResponse))]
        [SwaggerResponse(400, typeof(DrinkResponse))]
        public IActionResult UpdateDrink([FromBody] UpdateDrinkRequest request)
        {
            var response = _managementService.UpdateDrink(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("drink/{id}")]
        [SwaggerResponse(200, typeof(DrinkResponse))]
        [SwaggerResponse(400, typeof(DrinkResponse))]
        public IActionResult DeleteDrink(Guid id)
        {
            var response = _managementService.DeleteDrink(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
