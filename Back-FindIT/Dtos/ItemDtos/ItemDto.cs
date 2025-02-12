using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.UserDtos;
using Back_FindIT.Models;

namespace Back_FindIT.Dtos.ItemDtos
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public CategoryDto Category { get; set; } = null!;

        public UserDto RegisteredUser { get; set; } = null!;

        public UserDto ClaimedUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ItemDto(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Category = new CategoryDto {
                Id = item.Category.Id,
                Name = item.Category.Name
            };
            RegisteredUser = new UserDto {
                Id = item.RegisteredUser.Id,
                Name = item.RegisteredUser.Name,
                Email = item.RegisteredUser.Email
            };
            ClaimedUser = new UserDto {
                Id = item.ClaimedUser.Id,
                Name = item.ClaimedUser.Name,
                Email = item.ClaimedUser.Email
            };
            IsActive = item.IsActive;
            CreatedAt = item.CreatedAt;
            UpdatedAt = item.UpdatedAt;
        }
    }
}
