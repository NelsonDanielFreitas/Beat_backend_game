using Beat_backend_game.Migrations;
using Beat_backend_game.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Beat_backend_game.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Defina suas tabelas (DbSet)

        //dotnet ef migrations add InitialCreate
        //dotnet ef database update

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuração adicional, se necessário
        }
    }
}
