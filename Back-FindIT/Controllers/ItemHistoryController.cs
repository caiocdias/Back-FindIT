using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.ItemHistoryDtos;
using Back_FindIT.Models;
using Back_FindIT.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_FindIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemHistoryController : ControllerBase
    {
        private readonly ItemHistoryService _itemHistoryService;

        public ItemHistoryController(ItemHistoryService itemHistoryService)
        {
            _itemHistoryService = itemHistoryService;
        }

        [HttpPost("AddItemHistory")]
        public async Task<IActionResult> AddItemHistory(ItemHistory itemHistory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _itemHistoryService.AddItemHistoryAsync(itemHistory);
                return Ok(itemHistory);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
