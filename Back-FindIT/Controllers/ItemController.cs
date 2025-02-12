using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.ItemDtos;
using Back_FindIT.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_FindIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddItem([FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _itemService.AddItemAsync(itemDto);
                return Ok(item);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
