using Back_FindIT.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_FindIT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany()
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.RegisteredUser) // Um Item tem um RegisteredUser
                .WithMany() // RegisteredUser pode ter vários Itens, mas não há navegação explícita
                .HasForeignKey(i => i.RegisteredBy) // Chave estrangeira em Item
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ClaimedUser)
                .WithMany()
                .HasForeignKey(i => i.ClaimedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
