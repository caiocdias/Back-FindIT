using Back_FindIT.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.UserDtos;

namespace Back_FindIT.Dtos.ItemDtos
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual CategoryDto Category { get; set; } = null!;

        public virtual UserDto RegisteredUser { get; set; } = null!;

        public virtual UserDto ClaimedUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
