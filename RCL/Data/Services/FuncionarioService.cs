using MyColl.RCL.Data.Interfaces;
using RCL.Data.Model;
using System;

namespace MyColl.RCL.Data.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly AppDbContext _context; // Simula ou representa o contexto de BD

        public FuncionarioService(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // Gestão de Produtos
        // ============================

        public IEnumerable<Produto> ListarProdutos(Categoria categoria, DisponibilidadeProduto disponibilidade)
        {
            return _context.Produtos
                .Where(p => p.Categoria == categoria && p.Disponibilidade == disponibilidade)
                .ToList();
        }

        public async Task<Produto> AdicionarProdutoAsync(Produto produto)
        {
            produto.Estado = EstadoProduto.PendenteAprovacao;
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto?> EditarProdutoAsync(Produto produtoAtualizado)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == produtoAtualizado.Id);
            if (produto == null) return null;

            produto.Nome = produtoAtualizado.Nome;
            produto.Preco = produtoAtualizado.Preco;
            produto.Stock = produtoAtualizado.Stock;
            produto.Categoria = produtoAtualizado.Categoria;
            produto.Disponibilidade = produtoAtualizado.Disponibilidade;

            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<bool> ApagarProdutoAsync(int produtoId)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == produtoId);
            if (produto == null) return false;

            // Só pode apagar se não houver vendas relacionadas
            bool temVendas = _context.Encomendas
                .Any(e => e.Produtos.Any(p => p.ProdutoId == produtoId));

            if (temVendas) return false;

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtivarProdutoAsync(int produtoId)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == produtoId);
            if (produto == null) return false;

            produto.Disponibilidade = DisponibilidadeProduto.EmStock;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InativarProdutoAsync(int produtoId)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == produtoId);
            if (produto == null) return false;

            produto.Disponibilidade = DisponibilidadeProduto.Esgotado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtualizarPrecoEStockAsync(int produtoId, decimal novoPreco, int novoStock)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == produtoId);
            if (produto == null) return false;

            produto.PrecoBase = novoPreco;
            produto.Stock = novoStock;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlterarEstadoProdutoAsync(int produtoId, EstadoProduto novoEstado)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == produtoId);
            if (produto == null) return false;

            produto.Estado = novoEstado;
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================
        // Gestão de Utilizadores
        // ============================

        public async Task<bool> AtivarUtilizadorAsync(int utilizadorId)
        {
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.Id == utilizadorId);
            if (utilizador == null) return false;

            utilizador.Estado = EstadoUtilizador.Ativo;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InativarUtilizadorAsync(int utilizadorId)
        {
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.Id == utilizadorId);
            if (utilizador == null) return false;

            utilizador.Estado = EstadoUtilizador.Inativo;
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================
        // Gestão de Vendas / Encomendas
        // ============================

        public async Task<bool> ConfirmarVendaAsync(int encomendaId)
        {
            var encomenda = _context.Encomendas.FirstOrDefault(e => e.Id == encomendaId);
            if (encomenda == null) return false;

            encomenda.Estado = EstadoEncomenda.Confirmada;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejeitarVendaAsync(int encomendaId)
        {
            var encomenda = _context.Encomendas.FirstOrDefault(e => e.Id == encomendaId);
            if (encomenda == null) return false;

            encomenda.Estado = EstadoEncomenda.Rejeitada;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExpedirProdutoAsync(int encomendaId)
        {
            var encomenda = _context.Encomendas.FirstOrDefault(e => e.Id == encomendaId);
            if (encomenda == null) return false;

            encomenda.Estado = EstadoEncomenda.Expedida;

            // Atualiza stock dos produtos da encomenda
            foreach (var item in encomenda.Produtos)
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.Id == item.ProdutoId);
                if (produto != null)
                    produto.Stock -= item.Quantidade;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtualizarStockAposVendaAsync(int produtoId, int quantidadeVendida)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == produtoId);
            if (produto == null) return false;

            if (produto.Stock < quantidadeVendida)
                throw new InvalidOperationException("Stock insuficiente!");

            produto.Stock -= quantidadeVendida;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
