using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Beat_backend_game.Models
{
    public class VerdadeiroFalso
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pergunta")]
        public int IdPergunta { get; set; }
        public Question Pergunta { get; set; }

        public bool Correta { get; set; }

        public string? Curiosidade { get; set; }
    }
}
