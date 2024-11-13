using Beat_backend_game.DataAnnotation;
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
                RefreshToken = refreshToken
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest1 request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var user = await _userService.ValidateUserAsync(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var accessToken = _jwtTokenService.GenerateAccessToken(user.Id.ToString(), user.Username);
            var refreshToken = user.RefreshToken;

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
