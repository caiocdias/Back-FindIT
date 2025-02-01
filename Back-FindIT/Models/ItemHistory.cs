using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_FindIT.Models
{
    public class ItemHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public ActionType ActionType { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ItemDescription { get; set; }

        public DateTime ItemCreatedAt { get; set; }
        public DateTime ItemUpdatedAt { get; set; }

        [Required]
        public bool ItemIsActive { get; set; }

        [Required]
        public int ItemCategoryId { get; set; }

        [Required]
        public int ItemRegisteredBy { get; set; }

        [Required]
        public int ItemClaimedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; } = null!;
    }
}
