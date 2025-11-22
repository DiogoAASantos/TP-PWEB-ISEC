using MyColl.RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Net.Http.Json;

namespace MyColl.RCL.Data.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        // ============================
        // Gestão de Produtos
        // ============================

        public async Task<IEnumerable<Produto>> ListarProdutosAsync(Categoria categoria, DisponibilidadeProduto disponibilidade)
        {
            
        }

        public async Task<Produto> AdicionarProdutoAsync(Produto produto)
        {
            
        }

        public async Task<Produto?> EditarProdutoAsync(Produto produtoAtualizado)
        {
           
        }

        public async Task<bool> ApagarProdutoAsync(int produtoId)
        {
           
        }

        public async Task<bool> AtivarProdutoAsync(int produtoId)
        {
            
        }

        public async Task<bool> InativarProdutoAsync(int produtoId)
        {
           
        }

        public async Task<bool> AtualizarPrecoEStockAsync(int produtoId, decimal novoPreco, int novoStock)
        {
            
        }

        public async Task<bool> AlterarEstadoProdutoAsync(int produtoId, EstadoProduto novoEstado)
        {
            
        }

        // ============================
        // Gestão de Utilizadores
        // ============================

        public async Task<bool> AtivarUtilizadorAsync(int utilizadorId)
        {
           
        }

        public async Task<bool> InativarUtilizadorAsync(int utilizadorId)
        {
            
        }

        // ============================
        // Gestão de Encomendas / Vendas
        // ============================

        public async Task<bool> ConfirmarVendaAsync(int encomendaId)
        {
            
        }

        public async Task<bool> RejeitarVendaAsync(int encomendaId)
        {
           
        }

        public async Task<bool> ExpedirProdutoAsync(int encomendaId)
        {
          
        }

        public async Task<bool> AtualizarStockAposVendaAsync(int produtoId, int quantidadeVendida)
        {
            
        }
    }
}
