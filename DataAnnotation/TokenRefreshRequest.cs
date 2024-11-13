namespace Beat_backend_game.DataAnnotation
{
    public class TokenRefreshRequest
    {
        public string RefreshToken { get; set; }
    }

    public class TokenVerificationRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
