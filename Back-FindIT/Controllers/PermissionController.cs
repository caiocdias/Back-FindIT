using Back_FindIT.Dtos.Permission;
using Back_FindIT.Models;
using Back_FindIT.Services;
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


        [HttpGet("GetPermissionById/{id}")]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);

            if (permission == null)
                return NotFound(new { message = "Permissão não encontrada." });

            return Ok(permission);
        }
    }
}
