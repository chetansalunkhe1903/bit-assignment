using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginDto)
        {
            // TODO: Replace with real user validation
            if (loginDto.Username != "bituser" || loginDto.Password != "abc456")
                return Unauthorized("Invalid username or password");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, loginDto.Username),
            };

            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
