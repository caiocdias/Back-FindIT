using Back_FindIT.Data;
using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.PermissionDtos;
using Back_FindIT.Dtos.UserDtos;
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

        public async Task<List<CategoryDto>?> ListAllCategories()
        {
            var categories = await _appDbContext.Categories
                .Where(c => c.IsActive == true)
                .AsNoTracking()
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
        public async Task<bool> SoftDeleteCategoryAsync(int categoryId)
        {
            var category = await _appDbContext.Categories.FirstOrDefaultAsync(u => u.Id == categoryId);

            if (category == null)
                return false;

            if (!category.IsActive)
                return true;


            category.IsActive = false;
            category.SetUpdatedAt();

            _appDbContext.Categories.Update(category);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryDto.Id);

            if (category == null)
                return null;

            if (!category.IsActive)
                throw new UnauthorizedAccessException("Categoria desativada.");

            if (await _appDbContext.Users.AnyAsync(c => c.Name == categoryDto.Name && c.IsActive == true))
                throw new InvalidOperationException("Já existe uma categoria cadastrada com esse nome.");


            category.Name = categoryDto.Name;
            category.SetUpdatedAt();

            _appDbContext.Categories.Update(category);
            await _appDbContext.SaveChangesAsync();

            categoryDto.Name = category.Name;

            return categoryDto;
        }
    }
}
