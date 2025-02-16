using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.ItemDtos;
using Back_FindIT.Dtos.UserDtos;
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

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromBody] ItemRegisterDto itemRegisterDto)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _itemService.AddItemAsync(itemRegisterDto);
                return Ok(item);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("GetItemById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);

            if (item == null)
                return NotFound(new { message = "Item não encontrado." });

            if (!item.IsActive)
                return StatusCode(403, new { message = "Item desativado." });


            return Ok(item);
        }

        [HttpGet("GetItemByName/{name}")]
        public async Task<IActionResult> GetItemByName(string name)
        {
            var itens = await _itemService.GetItemByNameAsync(name);

            if (itens == null)
                return NotFound(new { message = "Item(s) não encontrado(s)." });

            return Ok(itens);
        }

        [HttpGet("GetItemByClaimedById/{id}")]
        public async Task<IActionResult> GetItemByClaimedById(int id)
        {
            var item = await _itemService.GetItemByClaimedByIdAsync(id);

            if (item == null)
                return NotFound(new { message = "Item(s) não encontrado(s)." });

            return Ok(item);
        }

        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            var item = await _itemService.GetAllItemsAsync();

            if (item == null)
                return NotFound(new { message = "Item(s) não encontrado(s)." });

            return Ok(item);
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var result = await _itemService.SoftDeleteItemAsync(id);

            if (!result)
                return NotFound(new { message = "Item não encontrado." });

            return Ok(new { message = "Item desativado com sucesso." });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedItem = await _itemService.UpdateItemAsync(id, itemDto);

                if (updatedItem == null)
                    return NotFound(new { message = "Item não encontrado." });

                return Ok(updatedItem);
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

        [HttpGet("Search/{query}")]
        public async Task<IActionResult> SearchItems(string query)
        {
            var items = await _itemService.SearchItemsAsync(query);

            if (items == null || !items.Any())
                return NotFound(new { message = "Nenhum item encontrado." });

            return Ok(items);
        }


        [HttpGet("GetSimilarItems/{itemId}")]
        public async Task<IActionResult> GetSimilarItems(int itemId)
        {
            var items = await _itemService.GetSimilarItemsAsync(itemId);

            if (items == null || !items.Any())
                return NotFound(new { message = "Nenhum item similar encontrado." });

            return Ok(items);
        }

    }
}
