using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public class FornecedorService : IFornecedorService
    {
        private readonly AppDbContext _context;

        public FornecedorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Fornecedor?> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(f => f.Email == email && f.Password == password && f.Estado == "Activo");

            return fornecedor;
        }

        public async Task<Produto> InserirProdutoAsync(int fornecedorId, Produto produto)
        {
            produto.FornecedorId = fornecedorId;
            produto.Estado = EstadoProduto.PendenteAprovacao;
            
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<List<Produto>> ConsultarProdutosAsync(int fornecedorId)
        {
            return await _context.Produtos
                .Where(p => p.FornecedorId == fornecedorId)
                .ToListAsync();
        }

        public async Task<Produto?> EditarProdutoAsync(int fornecedorId, Produto produtoAtualizado)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == produtoAtualizado.Id && p.FornecedorId == fornecedorId);

            if (produto == null) return null;

            // Atualiza os campos permitidos
            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.PrecoBase = produtoAtualizado.Preco;
            produto.Categoria = produtoAtualizado.Categoria;
            produto.Disponibilidade = produtoAtualizado.Disponibilidade;
            produto.SubCategoria = produtoAtualizado.SubCategoria;
            
            // Após edição, produto volta para pendente aprovação
            produto.Estado = EstadoProduto.PendenteAprovacao;

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<List<Venda>> ConsultarHistoricoVendasAsync(int fornecedorId)
        {
            return await _context.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .Where(v => v.Itens.Any(i => i.Produto.FornecedorId == fornecedorId))
                .ToListAsync();
        }
    }
}
