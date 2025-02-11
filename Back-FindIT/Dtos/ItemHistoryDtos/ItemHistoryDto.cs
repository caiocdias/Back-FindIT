using Back_FindIT.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Back_FindIT.Dtos.ItemHistoryDtos
{
    public class ItemHistoryDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public ActionType ActionType { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string? ItemDescription { get; set; }

        public DateTime ItemCreatedAt { get; set; }
        public DateTime ItemUpdatedAt { get; set; }

        public bool ItemIsActive { get; set; }

        public int ItemCategoryId { get; set; }

        public int ItemRegisteredBy { get; set; }

        public int ItemClaimedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
