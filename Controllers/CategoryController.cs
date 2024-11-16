using Beat_backend_game.DataAnnotation;
using Beat_backend_game.DataAnnotation.Category;
using Beat_backend_game.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beat_backend_game.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var (aceite, message) = await _categoryService.CreateCategoryAsync(request.CategoryName);

            if(aceite == true)
            {
                return Ok(new
                {
                    Message = message
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = message
                });
            }
            
        }

        [HttpPost("GetAllCategories")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(new
            {
                Categories = categories
            });
        }

    }
}
