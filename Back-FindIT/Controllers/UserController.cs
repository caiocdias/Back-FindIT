using Back_FindIT.Dtos;
using Back_FindIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_FindIT.Data;

namespace Back_FindIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public UserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// Adiciona um novo usuário ao sistema.
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserRegisterDto userDto)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("Dados inválidos. A senha não pode ser vazia.");

            // Verifica se o email já existe no banco
            var existingUser = await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
                return Conflict("Já existe um usuário cadastrado com esse e-mail.");

            // Verifica se o CPF já existe
            var existingCpf = await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Cpf == userDto.Cpf);
            if (existingCpf != null)
                return Conflict("Já existe um usuário cadastrado com esse CPF.");

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Cpf = userDto.Cpf,
                IsActive = true
            };

            user.SetPassword(userDto.Password);
            user.SetUpdatedAt();

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return Ok(new { message = "Usuário cadastrado com sucesso!", userId = user.Id });
        }
    }
}
