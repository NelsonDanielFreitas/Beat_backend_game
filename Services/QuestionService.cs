using Beat_backend_game.Data;
using Beat_backend_game.DTO;
using Beat_backend_game.Models;
using Microsoft.EntityFrameworkCore;
using static Beat_backend_game.DTO.QuestionDTO;

namespace Beat_backend_game.Services
{
    public class QuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddQuestion(Question pergunta, string tipo, object detalhes)
        {
            _context.Questions.Add(pergunta);
            await _context.SaveChangesAsync();

            switch (tipo)
            {
                case "Verdadeiro/Falso":
                    var verdadeiroFalso = (VerdadeiroFalso)detalhes;
                    verdadeiroFalso.IdPergunta = pergunta.Id;
                    _context.VerdadeiroFalsos.Add(verdadeiroFalso);
                    break;

                case "Escolha Múltipla":
                    var opcoes = (IEnumerable<EscolhaMultipla>)detalhes;
                    foreach (var opcao in opcoes)
                    {
                        opcao.IdPergunta = pergunta.Id;
                        _context.EscolhaMultiplas.Add(opcao);
                    }
                    break;

                case "Ordem de Palavras":
                    var palavras = (IEnumerable<OrdemPalavras>)detalhes;
                    foreach (var palavra in palavras)
                    {
                        palavra.IdPergunta = pergunta.Id;
                        _context.OrdemPalavras.Add(palavra);
                    }
                    break;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<QuestionDto>> GetAllQuestionAsync()
        {
            try
            {
                var questions = await _context.Questions
                    .Include(q => q.VerdadeiroFalsos)
                    .Include(q => q.EscolhaMultiplas)
                    .Include(q => q.OrdemPalavras)
                    .Select(q => new QuestionDto
                    {
                        Id = q.Id,
                        TextoPergunta = q.TextoPergunta,
                        TempoLimite = q.TempoLimite,
                        Categoria = q.Categoria,
                        NivelDificuldade = q.NivelDificuldade,
                        DataCriacao = q.DataCriacao,
                        DataUpdate = q.DataUpdate,
                        TipoPergunta = q.TipoPergunta,
                        VerdadeiroFalsos = q.VerdadeiroFalsos
                            .Select(vf => new VerdadeiroFalsoDto
                            {
                                Id = vf.Id,
                                Correta = vf.Correta
                            }).ToList(),
                        EscolhaMultiplas = q.EscolhaMultiplas
                            .Select(em => new EscolhaMultiplaDto
                            {
                                Id = em.Id,
                                TextoOpcao = em.TextoOpcao,
                                Correta = em.Correta
                            }).ToList(),
                        OrdemPalavras = q.OrdemPalavras
                            .Select(op => new OrdemPalavrasDTO
                            {
                                Id = op.Id,
                                Palavra = op.Palavra,
                                Posicao = op.Posicao
                            }).ToList()
                    }).ToListAsync();

                return questions;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar perguntas com detalhes", ex);
            }
        }


        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            // Busca a pergunta pelo ID
            var question = await _context.Questions
                .Include(q => q.VerdadeiroFalsos)
                .Include(q => q.EscolhaMultiplas)
                .Include(q => q.OrdemPalavras)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
                throw new Exception($"Pergunta com ID {questionId} não encontrada.");

            // Remove os registros relacionados
            _context.VerdadeiroFalsos.RemoveRange(question.VerdadeiroFalsos);
            _context.EscolhaMultiplas.RemoveRange(question.EscolhaMultiplas);
            _context.OrdemPalavras.RemoveRange(question.OrdemPalavras);

            // Remove a pergunta
            _context.Questions.Remove(question);

            // Salva as alterações no banco de dados
            var result = await _context.SaveChangesAsync();

            // Retorna true se alguma alteração foi feita
            return result > 0;
        }
    }
}
