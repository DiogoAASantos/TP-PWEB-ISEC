using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
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

        public async Task<Produto> InserirProdutoAsync(string fornecedorId, Produto produto, IBrowserFile? imagem)
        {
            produto.Id = 0;
            produto.FornecedorId = fornecedorId;
            produto.Estado = EstadoProduto.AVenda;

            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(produto.Nome ?? ""), "Nome");
            content.Add(new StringContent(produto.Descricao ?? ""), "Descricao");
            content.Add(new StringContent(produto.Preco.ToString(CultureInfo.InvariantCulture)), "Preco");
            content.Add(new StringContent(produto.Stock.ToString()), "Stock");
            content.Add(new StringContent(((int)produto.Tipo).ToString()), "Tipo");
            content.Add(new StringContent(produto.CategoriaId.ToString()), "CategoriaId");
            content.Add(new StringContent(produto.FornecedorId ?? ""), "FornecedorId");
            content.Add(new StringContent(((int)produto.Estado).ToString()), "Estado");

            if (imagem != null)
            {
                var fileContent = new StreamContent(imagem.OpenReadStream(1024 * 1024 * 5)); // 5MB
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imagem.ContentType);
                content.Add(fileContent, "imagem", imagem.Name);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "api/fornecedores/produtos"); 
            request.Content = content;

            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                throw new Exception($"Falha ao criar produto: {erro}");
            }

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
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/fornecedores/{fornecedorId}/produtos/{produtoAtualizado.Id}");

            request.Content = JsonContent.Create(produtoAtualizado);

            if (!string.IsNullOrEmpty(token))
            {
                var tokenLimpo = token.Trim().Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenLimpo);
                Console.WriteLine($">>>> SERVICE: Token injetado manualmente (PUT): {tokenLimpo.Substring(0, 10)}...");
            }
            else
            {
                Console.WriteLine(">>>> SERVICE: Erro - Token não encontrado no LocalStorage!");
            }

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Produto>();
        }

        public async Task<bool> AlterarEstadoProdutoAsync(string fornecedorId, int produtoId, EstadoProduto novoEstado)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/fornecedores/{fornecedorId}/produtos/{produtoId}/estado?novoEstado={novoEstado}");

            if (!string.IsNullOrEmpty(token))
            {
                var tokenLimpo = token.Trim().Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenLimpo);
                Console.WriteLine($">>>> SERVICE: Token injetado manualmente (PATCH): {tokenLimpo.Substring(0, 10)}...");
            }
            else
            {
                Console.WriteLine(">>>> SERVICE: Erro - Token não encontrado no LocalStorage!");
            }

            var response = await _http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<Produto?> ObterProdutoPorIdAsync(int produtoId)
        {
            return await _http.GetFromJsonAsync<Produto>($"/api/produtos/{produtoId}");
        }
    }
}
