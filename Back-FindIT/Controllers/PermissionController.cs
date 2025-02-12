using Back_FindIT.Dtos.PermissionDtos;
using Back_FindIT.Dtos.UserPermissionDtos;
using Back_FindIT.Models;
using Back_FindIT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_FindIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionService _permissionService;

        public PermissionController(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] PermissionRegisterDto permissionDto)
        {
            try
            {
                var permission = await _permissionService.AddPermissionAsync(permissionDto);
                return Ok(permission);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetPermissionById/{id}")]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);

            if (permission == null)
                return NotFound(new { message = "Permissão não encontrada." });

            return Ok(permission);
        }

        [Authorize]
        [HttpPut("UpdatePermission/{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPermission = await _permissionService.UpdatePermissionAsync(id, dto);

                if (updatedPermission == null)
                    return NotFound(new { message = "Permissão não encontrada." });

                return Ok(updatedPermission);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
