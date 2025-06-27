namespace Beat_backend_game.DataAnnotation.Question
{
    public class QuestionRequest
    {
        public int Id { get; set; }
        public string TextoPergunta { get; set; }
        public string Categoria { get; set; }
        public string TempoLimite { get; set; }
        public int NivelDificuldade { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataUpdate { get; set; }
        public string TipoPergunta { get; set; }
        public bool? Correta { get; set; }  
        public List<Opcao>? Opcoes { get; set; } 
        public string[]? Palavras { get; set; }
        public string? Curiosidade { get; set; }

        public class Opcao
        {
            public string TextoOpcao { get; set; } // Texto da opção
            public bool Correta { get; set; }     // Indica se a opção é correta
        }
    }
}
