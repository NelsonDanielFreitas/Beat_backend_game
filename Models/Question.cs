using System.ComponentModel.DataAnnotations;

namespace Beat_backend_game.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TextoPergunta { get; set; }
        [Required]
        public string TempoLimite { get; set; }
        [Required]
        public string Categoria { get; set; }
        [Required]
        public int NivelDificuldade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUpdate { get; set; }
        [Required]
        public string TipoPergunta { get; set; }

        public ICollection<VerdadeiroFalso> VerdadeiroFalsos { get; set; }
        public ICollection<EscolhaMultipla> EscolhaMultiplas { get; set; }
        public ICollection<OrdemPalavras> OrdemPalavras { get; set; }

    }
}
