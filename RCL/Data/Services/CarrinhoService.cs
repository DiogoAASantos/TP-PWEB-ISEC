using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RCL.Data.DTO;
using RCL.Data.DTO.CarrinhoDTOs;
using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Services
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IMyStorageService _localStorage;

        private List<CarrinhoItem> _carrinhoLocal = new();
        public IReadOnlyList<CarrinhoItem> Carrinho => _carrinhoLocal.AsReadOnly();

        public CarrinhoService(HttpClient http, AuthenticationStateProvider authenticationStateProvider, IMyStorageService localStorage)
        {
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        private async Task<string> GetUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("O utilizador não está autenticado.");
            }
            return userId;
        }

        private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string uri, object? content = null)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var request = new HttpRequestMessage(method, uri);

            if (content != null)
                request.Content = JsonContent.Create(content);

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));

            return request;
        }

        public async Task<List<CarrinhoItem>> ObterItensAsync()
        {
            string userId = await GetUserIdAsync();
            var request = await CreateRequestAsync(HttpMethod.Get, "api/carrinho/");
            var response = await _http.SendAsync(request);

            _carrinhoLocal.Clear();

            if (response.IsSuccessStatusCode)
            {
                var carrinhoDto = await response.Content.ReadFromJsonAsync<CarrinhoDTO>();
                if (carrinhoDto?.Itens != null)
                {
                    foreach (var dto in carrinhoDto.Itens)
                    {
                        _carrinhoLocal.Add(new CarrinhoItem
                        {
                            ClienteId = userId,
                            ProdutoId = dto.ProdutoId,
                            Quantidade = dto.Quantidade,
                            Produto = new Produto { Id = dto.ProdutoId, Nome = dto.Nome, Preco = dto.Preco }
                        });
                    }
                }
            }
            return _carrinhoLocal;
        }

        public async Task AdicionarProdutoAsync(int produtoId, int quantidade = 1)
        {
            string userId = await GetUserIdAsync();
            var dto = new AddCarrinhoDTO { UserId = userId, ProdutoId = produtoId, Quantidade = quantidade };

            var request = await CreateRequestAsync(HttpMethod.Post, "api/carrinho/adicionar", dto);
            await _http.SendAsync(request);
        }

        public async Task AtualizarQuantidadeAsync(int produtoId, int quantidade)
        {
            string userId = await GetUserIdAsync();
            var dto = new UpdateCarrinhoDTO { UserId = userId, ProdutoId = produtoId, Quantidade = quantidade };

            var request = await CreateRequestAsync(HttpMethod.Put, "api/carrinho/atualizar", dto);
            await _http.SendAsync(request);
        }

        public async Task RemoverProdutoAsync(int produtoId)
        {
            var request = await CreateRequestAsync(HttpMethod.Delete, $"api/carrinho/{produtoId}");
            await _http.SendAsync(request);
        }

        public async Task<decimal> TotalAsync()
        {
            var itens = await ObterItensAsync();
            return itens.Sum(item => item.Produto.Preco * item.Quantidade);
        }

        public async Task LimparCarrinho()
        {
            string userId = await GetUserIdAsync();
            var request = await CreateRequestAsync(HttpMethod.Delete, $"api/carrinho/{userId}/limpar");
            await _http.SendAsync(request);
        } 
    }
}

