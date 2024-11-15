using Microsoft.EntityFrameworkCore;

namespace MicroserviceVendas.Infra.Contexto
{
    public class VendasContext : DbContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }

        public DbSet<Venda> Vendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Venda>().HasKey(e => e.Id);
        }
    }
}
