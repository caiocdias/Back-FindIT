using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.UserDtos;
using Back_FindIT.Models;

namespace Back_FindIT.Dtos.ItemDtos
{
    public class ItemRegisterDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public CategoryDto Category { get; set; } = null!;

        public UserDto RegisteredUser { get; set; } = null!;

        public UserDto ClaimedUser { get; set; } = null!;
    }
}
