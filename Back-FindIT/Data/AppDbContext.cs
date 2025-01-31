using Back_FindIT.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_FindIT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> User { get; set; }
    }
}
