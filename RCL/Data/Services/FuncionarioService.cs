using MyColl.RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Net.Http.Json;

namespace MyColl.RCL.Data.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly HttpClient _http;

        public FuncionarioService(HttpClient http)
        {
            _http = http;
        }

        // ============================
        // Gestão de Produtos
        // ============================

        public async Task<IEnumerable<Produto>> ListarProdutosAsync(Categoria categoria, DisponibilidadeProduto disponibilidade)
        {
            var url = $"/api/produtos?categoria={categoria}&disponibilidade={disponibilidade}";
            return await _http.GetFromJsonAsync<List<Produto>>(url) ?? new List<Produto>();
        }

        public async Task<Produto> AdicionarProdutoAsync(Produto produto)
        {
            produto.Estado = EstadoProduto.PendenteAprovacao;
            var response = await _http.PostAsJsonAsync("/api/produtos", produto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Produto>() ?? produto;
        }

        public async Task<Produto?> EditarProdutoAsync(Produto produtoAtualizado)
        {
            var response = await _http.PutAsJsonAsync($"/api/produtos/{produtoAtualizado.Id}", produtoAtualizado);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Produto>();
        }

        public async Task<bool> ApagarProdutoAsync(int produtoId)
        {
            var response = await _http.DeleteAsync($"/api/produtos/{produtoId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AtivarProdutoAsync(int produtoId)
        {
            var response = await _http.PatchAsync($"/api/produtos/{produtoId}/ativar", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InativarProdutoAsync(int produtoId)
        {
            var response = await _http.PatchAsync($"/api/produtos/{produtoId}/inativar", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AtualizarPrecoEStockAsync(int produtoId, decimal novoPreco, int novoStock)
        {
            var response = await _http.PatchAsync(
                $"/api/produtos/{produtoId}/atualizar-preco-stock?novoPreco={novoPreco}&novoStock={novoStock}",
                null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AlterarEstadoProdutoAsync(int produtoId, EstadoProduto novoEstado)
        {
            var response = await _http.PatchAsync($"/api/produtos/{produtoId}/estado?novoEstado={novoEstado}", null);
            return response.IsSuccessStatusCode;
        }

        // ============================
        // Gestão de Utilizadores
        // ============================

        public async Task<bool> AtivarUtilizadorAsync(int utilizadorId)
        {
            var response = await _http.PatchAsync($"/api/utilizadores/{utilizadorId}/ativar", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InativarUtilizadorAsync(int utilizadorId)
        {
            var response = await _http.PatchAsync($"/api/utilizadores/{utilizadorId}/inativar", null);
            return response.IsSuccessStatusCode;
        }

        // ============================
        // Gestão de Encomendas / Vendas
        // ============================

        public async Task<bool> ConfirmarVendaAsync(int encomendaId)
        {
            var response = await _http.PatchAsync($"/api/encomendas/{encomendaId}/confirmar", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RejeitarVendaAsync(int encomendaId)
        {
            var response = await _http.PatchAsync($"/api/encomendas/{encomendaId}/rejeitar", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExpedirProdutoAsync(int encomendaId)
        {
            var response = await _http.PatchAsync($"/api/encomendas/{encomendaId}/expedir", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AtualizarStockAposVendaAsync(int produtoId, int quantidadeVendida)
        {
            var response = await _http.PatchAsync(
                $"/api/produtos/{produtoId}/atualizar-stock?quantidade={quantidadeVendida}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
