using System.ComponentModel.DataAnnotations;

namespace Beat_backend_game.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
