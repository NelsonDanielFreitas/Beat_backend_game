namespace Beat_backend_game.DTO
{
    public class QuestionDTO
    {
        public class QuestionDto
        {
            public int Id { get; set; }
            public string TextoPergunta { get; set; }
            public string TempoLimite { get; set; }
            public string Categoria { get; set; }
            public int NivelDificuldade { get; set; }
            public DateTime DataCriacao { get; set; }
            public DateTime DataUpdate { get; set; }
            public string TipoPergunta { get; set; }
            public List<VerdadeiroFalsoDto> VerdadeiroFalsos { get; set; }
            public List<EscolhaMultiplaDto> EscolhaMultiplas { get; set; }
            public List<OrdemPalavrasDTO> OrdemPalavras { get; set; }
        }

        public class VerdadeiroFalsoDto
        {
            public int Id { get; set; }
            public bool Correta { get; set; }
            public string Curiosidade { get; set; }
        }

        public class EscolhaMultiplaDto
        {
            public int Id { get; set; }
            public string TextoOpcao { get; set; }
            public bool Correta { get; set; }
        }

        public class OrdemPalavrasDTO
        {
            public int Id { get; set; }
            public string Palavra { get; set; }
            public int Posicao { get; set; }
        }

    }
}
