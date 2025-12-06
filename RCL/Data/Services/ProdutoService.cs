using RCL.Data.Model;
using RCL.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using RCL.Data.Model.Enums;

namespace RCL.Data.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly HttpClient _http;

        public ProdutoService(HttpClient http) 
        {
            _http = http;
        }

        public async Task<List<Produto>> ListarProdutosAsync()
        {
            // Chama endpoint da API
            return await _http.GetFromJsonAsync<List<Produto>>("/api/produtos/disponiveis") ?? new List<Produto>();
        }


        // Listar produtos por categoria, subcategoria, preço e disponibilidade
        public async Task<List<Produto>> ListarProdutosPorCategoriaAsync(string categoria, string? subcategoria = null,
                                                                         decimal? precoMin = null,
                                                                         decimal? precoMax = null,
                                                                         DisponibilidadeProduto? disponibilidade = null)
        {
            var url = $"/api/produto?categoria={Uri.EscapeDataString(categoria)}";

            if (!string.IsNullOrWhiteSpace(subcategoria))
                url += $"&subcategoria={Uri.EscapeDataString(subcategoria)}";

            if (precoMin.HasValue)
                url += $"&precoMin={precoMin.Value}";

            if (precoMax.HasValue)
                url += $"&precoMax={precoMax.Value}";

            if (disponibilidade.HasValue)
                url += $"&disponibilidade={disponibilidade.Value}";

            return await _http.GetFromJsonAsync<List<Produto>>(url) ?? new List<Produto>();
        }

        // Obter um produto em destaque (aleatório)
        public async Task<Produto?> ObterProdutoDestaqueAsync()
        {
            return await _http.GetFromJsonAsync<Produto?>("/api/produtos/destaque");
        }

        // Inserir novo produto
        public async Task<Produto> InserirProdutoAsync(string fornecedorId, Produto produto)
        {
            produto.FornecedorId = fornecedorId;
            produto.Estado = EstadoProduto.PendenteAprovacao;

            var response = await _http.PostAsJsonAsync("/api/fornecedores/produtos", produto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Produto>() ?? produto;
        }

        // Consultar produtos do fornecedor
        public async Task<List<Produto>> ConsultarProdutosAsync(string fornecedorId)
        {
            return await _http.GetFromJsonAsync<List<Produto>>($"/api/fornecedores/{fornecedorId}/produtos")
                   ?? new List<Produto>();
        }

        // Editar produto
        public async Task<Produto?> EditarProdutoAsync(string fornecedorId, Produto produtoAtualizado)
        {
            var response = await _http.PutAsJsonAsync($"/api/fornecedores/{fornecedorId}/produtos/{produtoAtualizado.Id}", produtoAtualizado);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Produto>();
        }

        // Alterar estado do produto
        public async Task<bool> AlterarEstadoProdutoAsync(string fornecedorId, int produtoId, EstadoProduto novoEstado)
        {
            var response = await _http.PatchAsync($"/api/fornecedores/{fornecedorId}/produtos/{produtoId}/estado?novoEstado={novoEstado}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<Produto?> ObterProdutoPorIdAsync(int produtoId)
        {
            return await _http.GetFromJsonAsync<Produto>($"/api/produtos/{produtoId}");
        }
    }
}
