using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;

namespace API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ============================
        // DbSets das entidades de negócio
        // ============================

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Encomenda> Encomendas { get; set; }
        public DbSet<EncomendaItem> EncomendaItems { get; set; }

        // Tabelas de detalhe para clientes e fornecedores
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Produto -> Fornecedor
            builder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Encomenda -> Cliente
            builder.Entity<Encomenda>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.HistoricoCompras)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // EncomendaItem -> Produto / Encomenda
            builder.Entity<EncomendaItem>()
                .HasOne(ei => ei.Produto)
                .WithMany()
                .HasForeignKey(ei => ei.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EncomendaItem>()
                .HasOne(ei => ei.Encomenda)
                .WithMany(e => e.Items)
                .HasForeignKey(ei => ei.EncomendaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}