using System.ComponentModel.DataAnnotations;

namespace Beat_backend_game.DataAnnotation
{
    public class LoginRequest1
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
