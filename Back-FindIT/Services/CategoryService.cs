using Back_FindIT.Data;
using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_FindIT.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _appDbContext;
        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CategoryDto?> AddCategoryAsync(CategoryDto categoryDto)
        {
            // Verifica se o e-mail já existe
            if (await _appDbContext.Categories.AnyAsync(u => u.Name == categoryDto.Name))
                throw new InvalidOperationException("Já existe uma categoria cadastrada com esse nome!");

            Category category = new Category
            {
                Name = categoryDto.Name,
                IsActive = true
            };

            category.SetUpdatedAt();

            _appDbContext.Categories.Add(category);
            await _appDbContext.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
