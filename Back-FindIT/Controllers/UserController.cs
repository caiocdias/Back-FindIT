using Back_FindIT.Services;
using Microsoft.AspNetCore.Mvc;
using Back_FindIT.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;

namespace Back_FindIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public UserController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserRegisterDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.AddUserAsync(userDto);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado." });

            if (!user.IsActive)
                return StatusCode(403, new { message = "Usuário desativado." });


            return Ok(user);
        }

        [Authorize]
        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado." });

            if (!user.IsActive)
                return StatusCode(403, new { message = "Usuário desativado." });


            return Ok(user);
        }

        [Authorize]
        [HttpGet("GetUserByName /{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var users = await _userService.GetUserByNameAsync(name);

            if (users == null)
                return NotFound(new { message = "Nenhum usuário encontrado" });

            return Ok(users);
        }

        [Authorize]
        [HttpGet("GetActiveUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var result = await _userService.SoftDeleteUserAsync(id);

            if (!result)
                return NotFound(new { message = "Usuário não encontrado." });

            return Ok(new { message = "Usuário desativado com sucesso." });
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, userDto);

                if (updatedUser == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                return Ok(updatedUser);
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var user = await _userService.GetUserEntityByEmailAsync(userDto.Email);
            if (user == null || !user.ValidatePassword(userDto.Password))
                return Unauthorized(new { message = "E-mail ou senha inválidos" });

            if (!user.IsActive)
                return StatusCode(403, new { message = "Usuário desativado" });

            var token = _jwtService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
