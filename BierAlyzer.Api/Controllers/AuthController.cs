using System;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using BierAlyzer.Api.Helper;
using BierAlyzer.Api.Services;
using BierAlyzer.Contracts.Communication.Auth;
using BierAlyzer.Contracts.Communication.Auth.Request;
using BierAlyzer.Contracts.Communication.Auth.Response;
using BierAlyzer.Contracts.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace BierAlyzer.Api.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <inheritdoc />
    ///  <summary>   Manage API token </summary>
    ///  <remarks>   Andre Beging, 18.06.2018. </remarks>
    [Route("api/auth/[action]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase, IAuthController<IActionResult>
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Token controller </summary>
        /// <remarks>   Andre Beging, 18.06.2018. </remarks>
        /// <param name="authService"></param>
        /// <param name="configuration">    The configuration. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get an API token pair </summary>
        /// <remarks>   Andre Beging, 18.06.2018. </remarks>
        /// <param name="request">  TokenRequest. </param>
        /// <returns>   Result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(TokenResponse), Description = "Fine, here is your token")]
        [SwaggerResponse(400, Description = "Invalid data")]
        [SwaggerResponse(500, Description = "Token creation problem")]
        public IActionResult Token([FromBody] TokenRequest request)
        {
            if (request == null) return BadRequest("Invalid data");
            var userClaims = _authService.ValidateCredentials(request.Mail, request.Password);

            if (userClaims != null)
            {
                // Create tokens
                var token = AuthenticationHelper.GenerateAccessToken(_configuration, userClaims);
                var refreshToken = AuthenticationHelper.GenerateRefreshToken(_configuration, userClaims);

                if (token == null) return StatusCode(500);
                if (refreshToken == null) return StatusCode(500);

                // Store refresh token
                var successfullyStored = _authService.StoreRefreshToken(refreshToken);
                if (!successfullyStored) return StatusCode(500);

                return Ok(new TokenResponse
                {
                    AccessToken = new TokenResource
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expires = token.ValidTo
                    },
                    RefreshToken = new TokenResource
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                        Expires = refreshToken.ValidTo
                    }
                });
            }

            return BadRequest();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a new pair of tokens using a refresh token </summary>
        /// <remarks>   Andre Beging, 09.11.2018. </remarks>
        /// <param name="refreshToken"> The refresh token. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(TokenResponse), Description = "Fine, here is your token")]
        [SwaggerResponse(400, Description = "Invalid data")]
        [SwaggerResponse(401, Description = "Refresh token expired")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest refreshToken)
        {
            try
            {
                var securityToken = new JwtSecurityToken(refreshToken.RefreshToken);
                // TODO Full token validation
                if (!AuthenticationHelper.ValidateLifetime(null, securityToken.ValidTo, null, null))
                    return Unauthorized();

                var userClaims = _authService.ValidateRefreshToken(securityToken);
                if (userClaims == null) return Unauthorized();

                // Create new tokens
                var newToken = AuthenticationHelper.GenerateAccessToken(_configuration, userClaims);
                var newRefreshToken = AuthenticationHelper.GenerateRefreshToken(_configuration, userClaims);

                if (newToken == null) return StatusCode(500);
                if (newRefreshToken == null) return StatusCode(500);

                // Store new refresh token
                var successfullyStored = _authService.StoreRefreshToken(newRefreshToken);
                if (!successfullyStored) return StatusCode(500);

                return Ok(new TokenResponse
                {
                    AccessToken = new TokenResource
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(newToken),
                        Expires = newToken.ValidTo
                    },
                    RefreshToken = new TokenResource
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(newRefreshToken),
                        Expires = newRefreshToken.ValidTo
                    }
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}