using Back_FindIT.Dtos.UserPermissionDtos;
using Back_FindIT.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_FindIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPermissionController : ControllerBase
    {
        private readonly UserPermissionService _userPermissionService;

        public UserPermissionController(UserPermissionService userPermissionService)
        {
            _userPermissionService = userPermissionService;
        }

        [HttpPost("AddUserPermission")]
        public async Task<IActionResult> AddUserPermission([FromBody] UserPermissionDto userPermissionDto)
        {
            try
            {
                bool success = await _userPermissionService.AddUserPermissionAsync(userPermissionDto.UserId, userPermissionDto.PermissionId);
                if (success)
                    return Ok(new { message = "Permissão atribuída com sucesso." });

                return BadRequest(new { message = "Erro ao atribuir permissão." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("RemoveUserPermission")]
        public async Task<IActionResult> RemoveUserPermission([FromBody] UserPermissionDto userPermissionDto)
        {
            try
            {
                bool success = await _userPermissionService.RemoveUserPermissionAsync(userPermissionDto.UserId, userPermissionDto.PermissionId);

                if (!success)
                    return NotFound(new { message = "Relação entre usuário e permissão não encontrada." });

                return Ok(new { message = "Permissão removida com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetPermissionsByUser/{userId}")]
        public async Task<IActionResult> GetPermissionsByUser(int userId)
        {
            var permissions = await _userPermissionService.GetPermissionsByUserAsync(userId);

            if (permissions == null || !permissions.Any())
                return NotFound(new { message = "Nenhuma permissão encontrada para este usuário." });

            return Ok(permissions);
        }

        [HttpGet("GetUsersByPermission/{permissionId}")]
        public async Task<IActionResult> GetUsersByPermission(int permissionId)
        {
            var users = await _userPermissionService.GetUsersByPermissionAsync(permissionId);

            if (users == null || !users.Any())
                return NotFound(new { message = "Nenhum usuário encontrado para esta permissão." });

            return Ok(users);
        }

    }
}
