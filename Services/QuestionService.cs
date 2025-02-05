using Beat_backend_game.Data;
using Beat_backend_game.DataAnnotation.Question;
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
                case "True/False":
                    var verdadeiroFalso = (VerdadeiroFalso)detalhes;
                    verdadeiroFalso.IdPergunta = pergunta.Id;
                    _context.VerdadeiroFalsos.Add(verdadeiroFalso);
                    break;

                case "Multiple Choice":
                    var opcoes = (IEnumerable<EscolhaMultipla>)detalhes;
                    foreach (var opcao in opcoes)
                    {
                        opcao.IdPergunta = pergunta.Id;
                        _context.EscolhaMultiplas.Add(opcao);
                    }
                    break;

                case "Word order":
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


        public async Task<List<QuestionDto>> GetAllQuestionCategoryAsync(string categoria)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoria))
                {
                    throw new ArgumentException("A categoria não pode estar vazia.");
                }

                var questions = await _context.Questions
                    .Where(q => q.Categoria == categoria) 
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
                throw new Exception("Erro ao buscar perguntas por categoria", ex);
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

        public async Task<bool> UpdateQuestionAsync(Question perguntaAtualizada, QuestionRequest request)
        {
            var perguntaExistente = await _context.Questions
                .Include(q => q.VerdadeiroFalsos)
                .Include(q => q.EscolhaMultiplas)
                .Include(q => q.OrdemPalavras)
                .FirstOrDefaultAsync(q => q.Id == perguntaAtualizada.Id);

            if (perguntaExistente == null)
                return false;

            perguntaExistente.TextoPergunta = perguntaAtualizada.TextoPergunta;
            perguntaExistente.TempoLimite = perguntaAtualizada.TempoLimite;
            perguntaExistente.Categoria = perguntaAtualizada.Categoria;
            perguntaExistente.NivelDificuldade = perguntaAtualizada.NivelDificuldade;
            perguntaExistente.DataUpdate = perguntaAtualizada.DataUpdate.Kind == DateTimeKind.Utc
                ? perguntaAtualizada.DataUpdate
                : perguntaAtualizada.DataUpdate.ToUniversalTime();

            try
            {
                switch (perguntaAtualizada.TipoPergunta)
                {
                    case "True/False":
                        var vfExistente = perguntaExistente.VerdadeiroFalsos.FirstOrDefault();
                        if (vfExistente != null)
                        {
                            vfExistente.Correta = request.Correta ?? false;
                        }
                        break;

                    case "Multiple Choice":
                        if (request.Opcoes != null)
                        {
                            var novasOpcoes = request.Opcoes.ToDictionary(o => o.TextoOpcao);

                            // Update or remove existing options
                            foreach (var opcaoExistente in perguntaExistente.EscolhaMultiplas.ToList())
                            {
                                if (novasOpcoes.TryGetValue(opcaoExistente.TextoOpcao, out var novaOpcao))
                                {
                                    // Update existing option
                                    opcaoExistente.Correta = novaOpcao.Correta;
                                    novasOpcoes.Remove(opcaoExistente.TextoOpcao);
                                }
                                else
                                {
                                    // Option no longer exists, remove it
                                    _context.EscolhaMultiplas.Remove(opcaoExistente);
                                }
                            }

                            // Add new options
                            foreach (var novaOpcao in novasOpcoes.Values)
                            {
                                _context.EscolhaMultiplas.Add(new EscolhaMultipla
                                {
                                    IdPergunta = perguntaAtualizada.Id,
                                    TextoOpcao = novaOpcao.TextoOpcao,
                                    Correta = novaOpcao.Correta
                                });
                            }
                        }
                        break;

                    case "Word order":
                        if (request.Palavras != null)
                        {
                            var palavrasAtualizadas = request.Palavras
                                .Select((palavra, index) => new { Palavra = palavra, Posicao = index })
                                .ToDictionary(p => p.Palavra);

                            // Update or remove existing words
                            foreach (var palavraExistente in perguntaExistente.OrdemPalavras.ToList())
                            {
                                if (palavrasAtualizadas.TryGetValue(palavraExistente.Palavra, out var novaPalavra))
                                {
                                    // Update position of existing word
                                    palavraExistente.Posicao = novaPalavra.Posicao;
                                    palavrasAtualizadas.Remove(palavraExistente.Palavra);
                                }
                                else
                                {
                                    // Word no longer exists, remove it
                                    _context.OrdemPalavras.Remove(palavraExistente);
                                }
                            }

                            // Add new words
                            foreach (var novaPalavra in palavrasAtualizadas.Values)
                            {
                                _context.OrdemPalavras.Add(new OrdemPalavras
                                {
                                    IdPergunta = perguntaAtualizada.Id,
                                    Palavra = novaPalavra.Palavra,
                                    Posicao = novaPalavra.Posicao
                                });
                            }
                        }
                        break;

                    default:
                        throw new Exception("Tipo de pergunta inválido.");
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Erro ao salvar alterações no banco: " + dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado: " + ex.Message);
            }
        }
    }
}
