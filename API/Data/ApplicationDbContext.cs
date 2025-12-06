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

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Encomenda> Encomendas { get; set; }
        public DbSet<EncomendaItem> EncomendaItems { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Produto -> Fornecedor
            builder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict); // Impede apagar Fornecedor se tiver produtos

            // 2. Encomenda -> Cliente
            builder.Entity<Encomenda>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.HistoricoCompras)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. EncomendaItem -> Encomenda
            builder.Entity<EncomendaItem>()
                .HasOne(ei => ei.Encomenda)
                .WithMany(e => e.Itens) // <--- CORRIGIDO: Era 'Items', mudámos para 'Itens' no Model
                .HasForeignKey(ei => ei.EncomendaId)
                .OnDelete(DeleteBehavior.Cascade); // Se apagar Encomenda, apaga os itens

            // 4. EncomendaItem -> Produto
            builder.Entity<EncomendaItem>()
                .HasOne(ei => ei.Produto)
                .WithMany()
                .HasForeignKey(ei => ei.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict); // Não apaga o item do histórico se o produto for apagado da loja

           
            // Define que os campos monetários têm 18 dígitos, sendo 2 casas decimais
            builder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Encomenda>()
                .Property(e => e.Total)
                .HasColumnType("decimal(18,2)");

            builder.Entity<EncomendaItem>()
                .Property(ei => ei.PrecoUnitario)
                .HasColumnType("decimal(18,2)");
        }
    }
}