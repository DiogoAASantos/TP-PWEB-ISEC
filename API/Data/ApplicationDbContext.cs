using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;
using System.Reflection.Emit;

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
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Categoria> Categoria { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Categoria>().HasData(
                new Categoria
                {
                    Id = 1,
                    Nome = "Moedas",
                    ImagemUrl = "/images/categorias/moedas.jpg"
                },
                new Categoria
                {
                    Id = 2,
                    Nome = "Selos",
                    ImagemUrl = "/images/categorias/selos.jpg"
                },
                new Categoria
                {
                    Id = 3,
                    Nome = "Carteiras de Fósforo",
                    ImagemUrl = "/images/categorias/carteiras_fosforos.jpg"
                },
                new Categoria
                {
                    Id = 4,
                    Nome = "Pacotes de Açúcar",
                    ImagemUrl = "/images/categorias/pacotes_açucar.jpg"
                },
                new Categoria
                {
                    Id = 5,
                    Nome = "Outros Coleccionáveis"
                }
            );






            builder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Encomenda>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.HistoricoCompras)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EncomendaItem>()
                .HasOne(ei => ei.Encomenda)
                .WithMany(e => e.Itens) 
                .HasForeignKey(ei => ei.EncomendaId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<EncomendaItem>()
                .HasOne(ei => ei.Produto)
                .WithMany()
                .HasForeignKey(ei => ei.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<CarrinhoItem>()
                .HasOne(ci => ci.Cliente)
                .WithMany() 
                .HasForeignKey(ci => ci.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<CarrinhoItem>()
                .HasOne(ci => ci.Produto)
                .WithMany()
                .HasForeignKey(ci => ci.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Categoria>()
                .HasOne(c => c.CategoriaPai)
                .WithMany(c => c.SubCategorias)
                .HasForeignKey(c => c.CategoriaPaiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

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