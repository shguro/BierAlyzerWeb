using System.Linq;
using BierAlyzer.Api.Services;
using BierAlyzer.Contracts.Communication.User.Request;
using BierAlyzer.Contracts.Communication.User.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BierAlyzer.Api.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Manage user profile. </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    [Route("api/user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <param name="userService">  The user service. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get the current user's profile. </summary>
        /// <returns>   The user profile. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("profile")]
        [SwaggerResponse(200, typeof(UserProfileResponse), "User profile")]
        [SwaggerResponse(400, typeof(UserProfileResponse), "Error")]
        public IActionResult GetProfile()
        {
            var response = _userService.GetUserProfile(HttpContext.User.Claims.ToArray());
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Update the current user's profile. </summary>
        /// <param name="request">  The update request. </param>
        /// <returns>   The updated user profile. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost("profile")]
        [SwaggerResponse(200, typeof(UserProfileResponse), "User profile updated")]
        [SwaggerResponse(400, typeof(UserProfileResponse), "Error")]
        public IActionResult UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            var response = _userService.UpdateUserProfile(request, HttpContext.User.Claims.ToArray());
            if (response.Result.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
