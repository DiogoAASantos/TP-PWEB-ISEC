using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public class FornecedorService : IFornecedorService
    {
        private readonly HttpClient _http;
        private readonly Fornecedor _fornecedor; // fornecedor atual (opcional)

        public FornecedorService(HttpClient http, Fornecedor fornecedor)
        {
            _http = http;
            _fornecedor = fornecedor;
        }

        // Login do fornecedor
        public async Task<Fornecedor?> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var response = await _http.PostAsJsonAsync("/api/fornecedores/login", new { email, password });
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Fornecedor>();
        }

        // Inserir novo produto
        public async Task<Produto> InserirProdutoAsync(int fornecedorId, Produto produto)
        {
            produto.FornecedorId = fornecedorId;
            produto.Estado = EstadoProduto.PendenteAprovacao;

            var response = await _http.PostAsJsonAsync("/api/fornecedores/produtos", produto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Produto>() ?? produto;
        }

        // Consultar produtos do fornecedor
        public async Task<List<Produto>> ConsultarProdutosAsync(int fornecedorId)
        {
            return await _http.GetFromJsonAsync<List<Produto>>($"/api/fornecedores/{fornecedorId}/produtos")
                   ?? new List<Produto>();
        }

        // Editar produto
        public async Task<Produto?> EditarProdutoAsync(int fornecedorId, Produto produtoAtualizado)
        {
            var response = await _http.PutAsJsonAsync($"/api/fornecedores/{fornecedorId}/produtos/{produtoAtualizado.Id}", produtoAtualizado);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Produto>();
        }

        // Alterar estado do produto
        public async Task<bool> AlterarEstadoProdutoAsync(int fornecedorId, int produtoId, EstadoProduto novoEstado)
        {
            var response = await _http.PatchAsync($"/api/fornecedores/{fornecedorId}/produtos/{produtoId}/estado?novoEstado={novoEstado}", null);
            return response.IsSuccessStatusCode;
        }

        // Consultar histórico de vendas
        public async Task<List<Encomenda>> ConsultarHistoricoVendasAsync(int fornecedorId)
        {
            // Chama a API para obter todas as encomendas que contenham produtos deste fornecedor
            return await _http.GetFromJsonAsync<List<Encomenda>>($"/api/fornecedores/{fornecedorId}/encomendas")
                   ?? new List<Encomenda>();
        }
    }
}
