using Back_FindIT.Controllers;
using Back_FindIT.Data;
using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.ItemDtos;
using Back_FindIT.Dtos.UserDtos;
using Back_FindIT.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_FindIT.Services
{
    public class ItemService
    {
        private readonly AppDbContext _appDbContext;
        public ItemService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ItemDto?> AddItemAsync(ItemDto itemDto)
        {
            // Verifica se 
            /*if (await _appDbContext.Items.AnyAsync(u => u.Name == categoryDto.Name))
                throw new InvalidOperationException("Já existe uma categoria cadastrada com esse nome!");*/
            int userId = 0;
            Item item = new Item
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                IsActive = true,
                CategoryId = itemDto.Category.Id,
                RegisteredBy = itemDto.RegisteredUser.Id
            };

            item.SetUpdatedAt();

            _appDbContext.Items.Add(item);
            await _appDbContext.SaveChangesAsync();

            itemDto.Id = item.Id;

            return itemDto;
        }
        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _appDbContext.Items
                .Include(i => i.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return null;

            return new ItemDto(item);
        }

        public async Task<List<ItemDto>?> GetItemByCategoryIdAsync(int categoryId)
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync();

            if (items == null)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }

        public async Task<List<ItemDto>?> GetItemByNameAsync(string name)
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .Where(i => EF.Functions.Like(i.Name, $"%{name}%") && i.IsActive)
                .ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }
        public async Task<List<ItemDto>?> GetItemByClaimedByIdAsync(int claimedById)
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .Where(i => i.ClaimedBy == claimedById)
                .ToListAsync();

            if (items == null)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }

        public async Task<List<ItemDto>?> GetAllItemsAsync()
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .ToListAsync();

            if (items == null)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }

        public async Task<bool> SoftDeleteItemAsync(int itemId)
        {
            var item = await _appDbContext.Items.FirstOrDefaultAsync(u => u.Id == itemId);

            if (item == null)
                return false;

            if (!item.IsActive)
                return true;


            item.IsActive = false;
            item.SetUpdatedAt();

            _appDbContext.Items.Update(item);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ItemDto?> UpdateItemAsync(int id, ItemDto itemDto)
        {
            var item = await _appDbContext.Items.FirstOrDefaultAsync(u => u.Id == itemDto.Id);

            if (item == null)
                return null;

            if (!item.IsActive)
                throw new UnauthorizedAccessException("Item desativado.");

            item.Name = itemDto.Name;
            item.Description = itemDto.Description;
            item.IsActive = itemDto.IsActive;
            item.CategoryId = itemDto.Category.Id;
            item.RegisteredBy = itemDto.RegisteredUser.Id;
            item.ClaimedBy = itemDto.ClaimedUser.Id;
            item.SetUpdatedAt();

            _appDbContext.Items.Update(item);
            await _appDbContext.SaveChangesAsync();

            return new ItemDto(item);
        }
    }
}
