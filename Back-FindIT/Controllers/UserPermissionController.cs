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

    }
}
