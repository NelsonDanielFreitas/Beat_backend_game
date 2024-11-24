using Beat_backend_game.Services;

namespace Beat_backend_game.Middleware
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserService userService, JwtTokenService jwtTokenService)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var accessToken = authHeader.Split(' ')[1];

                if (!jwtTokenService.ValidateAccessToken(accessToken))
                {
                    var userId = jwtTokenService.GetUserIdFromExpiredToken(accessToken); 

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var user = await userService.GetUserByIdAsync(userId);

                        if (user != null && user.RefreshTokenExpiry > DateTime.UtcNow)
                        {
                           
                            string role = user.IsAdmin ? "Admin" : "User";
                            var newAccessToken = jwtTokenService.GenerateAccessToken(user.Id.ToString(), user.Username, role);

                            context.Response.Headers["New-AccessToken"] = newAccessToken;

                            context.Request.Headers["Authorization"] = $"Bearer {newAccessToken}";
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
