using Beat_backend_game.DataAnnotation;
using Beat_backend_game.Models;
using Beat_backend_game.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Beat_backend_game.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(UserService userService, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest1 request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            // Tenta criar um novo usuário
            var (user, refreshToken) = await _userService.RegisterUserAsync(request.Username, request.Email, request.Password);

            if (user == null)
            {
                return BadRequest("Username or email already exists");
            }

            // Gera o Access Token
            var accessToken = _jwtTokenService.GenerateAccessToken(user.Id.ToString(), user.Username);

            return Ok(new
            {
                AccessToken = accessToken,
                User = user
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest1 request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data");

            var user = await _userService.ValidateUserAsync(request.Username, request.Password);
            if (user == null) return Unauthorized("Invalid username or password");

            var accessToken = _jwtTokenService.GenerateAccessToken(user.Id.ToString(), user.Username);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = _jwtTokenService.GetRefreshTokenExpiry();
            await _userService.UpdateUserAsync(user);

            var UserSend = new User
            {
                Username = user.Username,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };

            return Ok(new { AccessToken = accessToken, User = UserSend });
        }

        [HttpPost("validate-refresh-token")]
        public async Task<IActionResult> ValidateRefreshToken([FromBody] TokenRefreshRequest request)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            return Ok("Refresh token is valid");
        }

        [HttpPost("verify-access-token")]
        public async Task<IActionResult> VerifyAccessToken([FromBody] TokenVerificationRequest request)
        {
            var isAccessTokenValid = _jwtTokenService.ValidateAccessToken(request.AccessToken);
            if (isAccessTokenValid)
            {
                return Ok("Access token is valid");
            }

            // If the access token is not valid, validate the refresh token
            var user = await _userService.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            // Generate a new access token if the refresh token is valid
            var newAccessToken = _jwtTokenService.GenerateAccessToken(user.Id.ToString(), user.Username);
            return Ok(new { AccessToken = newAccessToken });
        }
    }
}
