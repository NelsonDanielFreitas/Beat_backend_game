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
        public DbSet<Question> Questions { get; set; }
        public DbSet<VerdadeiroFalso> VerdadeiroFalsos { get; set; }
        public DbSet<EscolhaMultipla> EscolhaMultiplas { get; set; }
        public DbSet<OrdemPalavras> OrdemPalavras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuração adicional, se necessário
            modelBuilder.Entity<Question>()
                .HasMany(p => p.VerdadeiroFalsos)
                .WithOne(vf => vf.Pergunta)
                .HasForeignKey(vf => vf.IdPergunta);

            modelBuilder.Entity<Question>()
                .HasMany(p => p.EscolhaMultiplas)
                .WithOne(em => em.Pergunta)
                .HasForeignKey(em => em.IdPergunta);

            modelBuilder.Entity<Question>()
                .HasMany(p => p.OrdemPalavras)
                .WithOne(op => op.Pergunta)
                .HasForeignKey(op => op.IdPergunta);
        }
    }
}
