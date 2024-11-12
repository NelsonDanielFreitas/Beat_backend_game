using Beat_backend_game.Models;
using Microsoft.EntityFrameworkCore;

namespace Beat_backend_game.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Defina suas tabelas (DbSet)
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuração adicional, se necessário
        }
    }
}
