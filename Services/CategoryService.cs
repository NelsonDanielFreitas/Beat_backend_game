using Beat_backend_game.Data;
using Beat_backend_game.Models;
using Microsoft.EntityFrameworkCore;

namespace Beat_backend_game.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool aceite, string message)> CreateCategoryAsync(string category)
        {
            if (await _context.Categories.AnyAsync(u => u.CategoryName == category))
            {
                return (false, "Categoria já existe"); // Você pode retornar uma mensagem personalizada aqui
            }

            var category1 = new Category
            {
                CategoryName = category
            };

            // Salva o usuário no banco de dados
            _context.Categories.Add(category1);
            await _context.SaveChangesAsync();

            return (true, "Categoria criada com sucesso");
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                // Você pode logar o erro em um arquivo de log
                throw new Exception("Erro ao buscar categorias", ex);
            }
        }
    }
}
