using Microsoft.EntityFrameworkCore;

namespace MicroservicePrecos.Infra.Contexto
{
    public class PrecosContext : DbContext
    {
        public PrecosContext(DbContextOptions<PrecosContext> options) : base(options) { }

        public DbSet<Preco> Precos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Preco>().HasKey(e => e.Id);
        }
    }
}