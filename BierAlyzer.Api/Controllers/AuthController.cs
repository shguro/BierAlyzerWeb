using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IUserService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Login with username and password.
        /// </summary>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login with username and password.")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _service.LoginAsync(dto);
            if (result == null) return Unauthorized();
            return Ok(result);
        }
    }
}
