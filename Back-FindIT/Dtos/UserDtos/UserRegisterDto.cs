using System.ComponentModel.DataAnnotations;

namespace Back_FindIT.Dtos.UserDtos
{
    public class UserRegisterDto
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }
}
