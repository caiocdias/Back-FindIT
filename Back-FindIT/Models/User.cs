using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Back_FindIT.Models
{
    public class User
    {
        private const int SaltSize = 16;  // 16 bytes de salt
        private const int KeySize = 32;   // 32 bytes para o hash
        private const int Iterations = 100_000; // Número de iterações PBKDF2

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
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;

        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new HashSet<UserPermission>();

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.Now;
        }

        public void SetPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                // Gera um salt aleatório
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                // Usa PBKDF2 para gerar um hash seguro
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512))
                {
                    byte[] key = pbkdf2.GetBytes(KeySize);

                    // Armazena o salt concatenado com o hash
                    PasswordHash = new byte[SaltSize + KeySize];
                    Array.Copy(salt, 0, PasswordHash, 0, SaltSize);
                    Array.Copy(key, 0, PasswordHash, SaltSize, KeySize);
                }
            }
        }

        public bool ValidatePassword(string password)
        {
            if (PasswordHash == null || PasswordHash.Length != SaltSize + KeySize)
                return false;

            // Extrai o salt do hash armazenado
            byte[] salt = new byte[SaltSize];
            Array.Copy(PasswordHash, 0, salt, 0, SaltSize);

            // Recalcula o hash usando o salt extraído
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512))
            {
                byte[] computedKey = pbkdf2.GetBytes(KeySize);

                // Compara os hashes de forma segura
                for (int i = 0; i < KeySize; i++)
                {
                    if (computedKey[i] != PasswordHash[SaltSize + i])
                        return false;
                }
                return true;
            }
        }
    }
}