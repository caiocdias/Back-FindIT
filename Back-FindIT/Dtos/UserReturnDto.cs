using Back_FindIT.Models;

namespace Back_FindIT.Dtos
{
    public class UserReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new HashSet<UserPermission>();
    }
}
