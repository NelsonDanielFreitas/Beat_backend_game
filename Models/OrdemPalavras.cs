using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Beat_backend_game.Models
{
    public class OrdemPalavras
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pergunta")]
        public int IdPergunta { get; set; }
        public Question Pergunta { get; set; }

        [Required]
        public string Palavra { get; set; }

        [Required]
        public int Posicao { get; set; }
    }
}
