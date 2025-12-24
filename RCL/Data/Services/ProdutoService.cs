using Blazored.LocalStorage;
using RCL.Data.Interfaces;
using RCL.Data.Model;
using RCL.Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RCL.Data.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly HttpClient _http;
        private readonly IMyStorageService _localStorage;

        public ProdutoService(HttpClient http, IMyStorageService localStorage) 
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<List<Produto>> ListarProdutosAsync()
        {
            // Chama endpoint da API
            return await _http.GetFromJsonAsync<List<Produto>>("/api/produtos/disponiveis") ?? new List<Produto>();
        }


        // Listar produtos por categoria, subcategoria, preço e disponibilidade
        public async Task<List<Produto>> ListarProdutosPorCategoriaAsync(int categoriaId, int? subcategoriaId = null,
                                                                         decimal? precoMin = null,
                                                                         decimal? precoMax = null,
                                                                         DisponibilidadeProduto? disponibilidade = null)
        {
            // 1. URL Base com o ID obrigatório da Categoria
            // Nota: A rota deve bater certo com o "HttpGet("listar")" que definimos na API
            var url = $"api/produtos/listar?categoriaId={categoriaId}";

            // 2. Subcategoria (Agora é INT, verificamos se tem valor)
            if (subcategoriaId.HasValue)
            {
                url += $"&subcategoriaId={subcategoriaId.Value}";
            }

            // 3. Preços (Formatados com PONTO para não dar erro de URL)
            if (precoMin.HasValue)
            {
                url += $"&precoMin={precoMin.Value.ToString(CultureInfo.InvariantCulture)}";
            }

            if (precoMax.HasValue)
            {
                url += $"&precoMax={precoMax.Value.ToString(CultureInfo.InvariantCulture)}";
            }

            // 4. Disponibilidade (Enum enviado como Inteiro)
            if (disponibilidade.HasValue)
            {
                url += $"&disponibilidade={(int)disponibilidade.Value}";
            }

            // 5. Chamada à API
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
            produto.Id = 0;
            produto.FornecedorId = fornecedorId;
            produto.Estado = EstadoProduto.AVenda;

            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/fornecedores/produtos");

            request.Content = JsonContent.Create(produto);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Produto>() ?? produto;
        }

        // Consultar produtos do fornecedor
        public async Task<List<Produto>> ConsultarProdutosAsync(string fornecedorId)
        {
            // 1. Buscamos o token diretamente aqui (onde o JSInterop funciona)
            var token = await _localStorage.GetItemAsync<string>("authToken");

            // 2. Criamos a mensagem de pedido manualmente
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/fornecedores/{fornecedorId}/produtos");

            if (!string.IsNullOrEmpty(token))
            {
                // 3. Injetamos o token "na mão" limpando as aspas
                var tokenLimpo = token.Trim().Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenLimpo);
                Console.WriteLine($">>>> SERVICE: Token injetado manualmente: {tokenLimpo.Substring(0, 10)}...");
            }
            else
            {
                Console.WriteLine(">>>> SERVICE: Erro - Token não encontrado no LocalStorage!");
            }

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Produto>>() ?? new List<Produto>();
            }

            var erro = await response.Content.ReadAsStringAsync();

            return new List<Produto>();
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
