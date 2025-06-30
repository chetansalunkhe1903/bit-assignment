using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "Login to get Auth token")]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                // Simulated authentication (replace with real user validation)
                if (loginDto.Username != "bituser" || loginDto.Password != "abc456")
                {
                    _logger.LogWarning("Login failed for user: {Username}", loginDto.Username);
                    return Unauthorized("Invalid username or password");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, loginDto.Username),
                };

                var accessToken = _jwtTokenService.GenerateAccessToken(claims);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                _logger.LogInformation("User '{Username}' logged in successfully.", loginDto.Username);

                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during login for user: {Username}", loginDto.Username);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
