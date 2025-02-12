using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.UserDtos;
using Back_FindIT.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_FindIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var category = await _categoryService.AddCategoryAsync(categoryDto);
                return Ok(category);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("ListAllCategories")]
        public async Task<IActionResult> ListAllCategories()
        {
            var categories = await _categoryService.ListAllCategories();

            if (categories == null)
                return NotFound(new { message = "Categorias não encontradas." });

            return Ok(categories);
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var result = await _categoryService.SoftDeleteCategoryAsync(id);

            if (!result)
                return NotFound(new { message = "Categoria não encontrada." });

            return Ok(new { message = "Categoria desativada com sucesso." });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(category);

                if (updatedCategory == null)
                    return NotFound(new { message = "Categoria não encontrada." });

                return Ok(updatedCategory);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }
    }
}
