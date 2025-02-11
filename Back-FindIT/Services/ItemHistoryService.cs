using Back_FindIT.Data;
using Back_FindIT.Dtos.ItemDtos;
using Back_FindIT.Models;

namespace Back_FindIT.Services
{
    public class ItemHistoryService
    {
        private readonly AppDbContext _appDbContext;
        public ItemHistoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ItemHistory?> AddItemHistoryAsync(ItemHistory itemHistory)
        {
            _appDbContext.ItemHistories.Add(itemHistory);
            await _appDbContext.SaveChangesAsync();

            return itemHistory;
        }
    }
}
