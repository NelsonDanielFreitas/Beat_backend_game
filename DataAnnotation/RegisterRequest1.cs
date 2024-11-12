using System.ComponentModel.DataAnnotations;

namespace Beat_backend_game.DataAnnotation
{
    public class RegisterRequest1
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, with at least one uppercase, one lowercase, one digit, and one special character.")]
        public string Password { get; set; }
    }
}
