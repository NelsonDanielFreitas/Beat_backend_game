using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Beat_backend_game.Models
{
    public class EscolhaMultipla
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pergunta")]
        public int IdPergunta { get; set; }
        public Question Pergunta { get; set; }

        [Required]
        public string TextoOpcao { get; set; }

        [Required]
        public bool Correta { get; set; }
    }
}
