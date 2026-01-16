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
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Administration and management. </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    [Route("api/management")]
    [Authorize]
    public class ManagementController : Controller
    {
        private readonly ManagementService _managementService;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <param name="managementService">    The management service. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ManagementController(ManagementService managementService)
        {
            _managementService = managementService;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get all users (Admin only). </summary>
        /// <returns>   All users. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("users")]
        [SwaggerResponse(200, typeof(UsersResponse))]
        [SwaggerResponse(400, typeof(UsersResponse))]
        public IActionResult GetUsers()
        {
            var response = _managementService.GetUsers(HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a specific user by ID (Admin only). </summary>
        /// <param name="id">   The user identifier. </param>
        /// <returns>   The user. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("user/{id}")]
        [SwaggerResponse(200, typeof(UsersResponse))]
        [SwaggerResponse(400, typeof(UsersResponse))]
        public IActionResult GetUser(Guid id)
        {
            var response = _managementService.GetUser(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Update a user (Admin only). </summary>
        /// <param name="request">  The update request. </param>
        /// <returns>   The updated user. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost("user")]
        [SwaggerResponse(200, typeof(UsersResponse))]
        [SwaggerResponse(400, typeof(UsersResponse))]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            var response = _managementService.UpdateUser(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get all events (Admin only). </summary>
        /// <returns>   All events. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("events")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult GetEvents()
        {
            var response = _managementService.GetAllEvents(HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Delete an event (Admin only). </summary>
        /// <param name="id">   The event identifier. </param>
        /// <returns>   Result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpDelete("event/{id}")]
        [SwaggerResponse(200, typeof(EventResponse))]
        [SwaggerResponse(400, typeof(EventResponse))]
        public IActionResult DeleteEvent(Guid id)
        {
            var response = _managementService.DeleteEvent(id, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get all drinks (Admin only). </summary>
        /// <returns>   All drinks. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("drinks")]
        [SwaggerResponse(200, typeof(DrinkResponse))]
        [SwaggerResponse(400, typeof(DrinkResponse))]
        public IActionResult GetDrinks()
        {
            var response = _managementService.GetDrinks(HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Create a new drink (Admin only). </summary>
        /// <param name="request">  The create request. </param>
        /// <returns>   The created drink. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost("drink")]
        [SwaggerResponse(200, typeof(DrinkResponse))]
        [SwaggerResponse(400, typeof(DrinkResponse))]
        public IActionResult CreateDrink([FromBody] CreateDrinkRequest request)
        {
            var response = _managementService.CreateDrink(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Update a drink (Admin only). </summary>
        /// <param name="request">  The update request. </param>
        /// <returns>   The updated drink. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPut("drink")]
        [SwaggerResponse(200, typeof(DrinkResponse))]
        [SwaggerResponse(400, typeof(DrinkResponse))]
        public IActionResult UpdateDrink([FromBody] UpdateDrinkRequest request)
        {
            var response = _managementService.UpdateDrink(request, HttpContext.User.Claims);
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Delete a drink (Admin only). </summary>
        /// <param name="id">   The drink identifier. </param>
        /// <returns>   Result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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
