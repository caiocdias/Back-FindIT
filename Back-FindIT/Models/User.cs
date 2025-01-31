using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Back_FindIT.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = new byte[0];

        [Required]
        public byte[] PasswordSalt { get; set; } = new byte[0];

        [Required]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;

        [Column("created_at", TypeName = "DATETIME"), Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key; // Armazena o salt gerado
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Cria o hash
            }
        }

        public bool ValidatePassword(string password)
        {
            if (PasswordSalt == null || PasswordHash == null) return false;

            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return StructuralComparisons.StructuralEqualityComparer.Equals(computedHash, PasswordHash);
            }
        }
    }
}
