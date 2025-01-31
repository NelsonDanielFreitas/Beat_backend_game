using Beat_backend_game.DataAnnotation.Question;
using Beat_backend_game.Models;
using Beat_backend_game.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beat_backend_game.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly QuestionService _questionService;

        public QuestionController(QuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("CreateQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pergunta = new Question
            {
                TextoPergunta = request.TextoPergunta,
                TempoLimite = request.TempoLimite,
                Categoria = request.Categoria,
                NivelDificuldade = request.NivelDificuldade,
                DataCriacao = request.DataCriacao ?? DateTime.Now,
                DataUpdate = request.DataUpdate ?? DateTime.Now,
                TipoPergunta = request.TipoPergunta
            };

            switch (request.TipoPergunta)
            {
                case "Verdadeiro/Falso":
                    var verdadeiroFalso = new VerdadeiroFalso
                    {
                        Correta = request.Correta ?? false
                    };
                    await _questionService.AddQuestion(pergunta, "Verdadeiro/Falso", verdadeiroFalso);
                    break;

                case "Escolha Múltipla":
                    var opcoes = request.Opcoes?.Select(o => new EscolhaMultipla
                    {
                        TextoOpcao = o.TextoOpcao,
                        Correta = o.Correta
                    }).ToList();
                    await _questionService.AddQuestion(pergunta, "Escolha Múltipla", opcoes);
                    break;

                case "Ordem de Palavras":
                    var palavras = request.Palavras?.Select((palavra, index) => new OrdemPalavras
                    {
                        Palavra = palavra,
                        Posicao = index
                    }).ToList();
                    await _questionService.AddQuestion(pergunta, "Ordem de Palavras", palavras);
                    break;

                default:
                    return BadRequest("Tipo de pergunta inválido.");
            }

            return Ok(new { Message = "Pergunta criada com sucesso." });
        }

        [HttpGet("GetAllQuestion")]
        public async Task<IActionResult> GetAllQuestion()
        {
            var questions = _questionService.GetAllQuestionAsync();

            return Ok(new
            {
                Questions = questions
            });
        }

        [HttpGet("GetAllQuestionCategory/{category}")]
        public async Task<IActionResult> GetAllQuestionCategory(string category)
        {
            var question = _questionService.GetAllQuestionCategoryAsync(category);
            return Ok(new
            {
                Questions = question
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                var result = await _questionService.DeleteQuestionAsync(id);

                if (!result)
                    return NotFound(new { message = $"Pergunta com ID {id} não encontrada." });

                return Ok(new { message = $"Pergunta com ID {id} foi removida com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("UpdateQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Atualiza a pergunta principal
                var perguntaAtualizada = new Question
                {
                    Id = request.Id,
                    TextoPergunta = request.TextoPergunta,
                    TempoLimite = request.TempoLimite,
                    Categoria = request.Categoria,
                    NivelDificuldade = request.NivelDificuldade,
                    DataUpdate = DateTime.Now,
                    TipoPergunta = request.TipoPergunta
                };

                // Chama o service para atualizar a pergunta
                var result = await _questionService.UpdateQuestionAsync(perguntaAtualizada, request);

                if (!result)
                    return NotFound(new { message = $"Pergunta com ID {request.Id} não encontrada." });

                return Ok(new { Message = "Pergunta atualizada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erro ao atualizar pergunta: {ex.Message}" });
            }
        }
    }
}
