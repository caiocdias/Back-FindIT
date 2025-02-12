using Back_FindIT.Data;
using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.ItemDtos;
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
    }
}
