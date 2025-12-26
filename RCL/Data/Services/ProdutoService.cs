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
            return await _http.GetFromJsonAsync<List<Produto>>("/api/produtos/disponiveis") ?? new List<Produto>();
        }


        public async Task<List<Produto>> ListarProdutosPorCategoriaAsync(int categoriaId)
        {
            var url = $"api/produtos/categoria/{categoriaId}";
            return await _http.GetFromJsonAsync<List<Produto>>(url) ?? new List<Produto>();
        }

        public async Task<Produto?> ObterProdutoDestaqueAsync()
        {
            return await _http.GetFromJsonAsync<Produto?>("/api/produtos/destaque");
        }

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

        public async Task<List<Produto>> ConsultarProdutosAsync(string fornecedorId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/fornecedores/{fornecedorId}/produtos");

            if (!string.IsNullOrEmpty(token))
            {
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

        public async Task<Produto?> EditarProdutoAsync(string fornecedorId, Produto produtoAtualizado)
        {
            var response = await _http.PutAsJsonAsync($"/api/fornecedores/{fornecedorId}/produtos/{produtoAtualizado.Id}", produtoAtualizado);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Produto>();
        }

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
