using Microsoft.AspNetCore.Components.Authorization;
using RCL.Data.DTO;
using RCL.Data.DTO.CarrinhoDTOs;
using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<CarrinhoItem> _carrinhoLocal = new();
        public IReadOnlyList<CarrinhoItem> Carrinho => _carrinhoLocal.AsReadOnly();

        public CarrinhoService(HttpClient http, AuthenticationStateProvider authenticationStateProvider)
        {
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
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

        public async Task<List<CarrinhoItem>> ObterItensAsync()
        {
            string userId = await GetUserIdAsync(); 
            var carrinhoDto = await _http.GetFromJsonAsync<CarrinhoDTO>($"api/carrinho/");

            _carrinhoLocal.Clear();

            if (carrinhoDto?.Itens != null)
            {
                foreach (var dto in carrinhoDto.Itens)
                {
                    var novoItem = new CarrinhoItem
                    {
                        ClienteId = userId,
                        ProdutoId = dto.ProdutoId,
                        Quantidade = dto.Quantidade,
                        Produto = new Produto { Id = dto.ProdutoId, Nome = dto.Nome, Preco = dto.Preco }
                    };

                    _carrinhoLocal.Add(novoItem); 
                }
            }

            return _carrinhoLocal; 
        }

        public void LimparCarrinhoLocal()
        {
            _carrinhoLocal.Clear();
        }

        public async Task AdicionarProdutoAsync(string produtoId, int quantidade = 1)
        {
            string userId = await GetUserIdAsync();
            var dto = new AddCarrinhoDTO
            {
                UserId = userId,
                ProdutoId = produtoId,
                Quantidade = quantidade
            };

            await _http.PostAsJsonAsync("api/carrinho/adicionar", dto);
        }

        public async Task AtualizarQuantidadeAsync(string produtoId, int quantidade)
        {
            string userId = await GetUserIdAsync();
            var dto = new UpdateCarrinhoDTO
            {
                UserId = userId,
                ProdutoId = produtoId,
                Quantidade = quantidade
            };

            await _http.PutAsJsonAsync("api/carrinho/atualizar", dto);
        }

        public async Task RemoverProdutoAsync(string produtoId)
        {
            string userId = await GetUserIdAsync();
            await _http.DeleteAsync($"api/carrinho/{produtoId}");
        }

        public async Task<decimal> TotalAsync()
        {
            var itens = await ObterItensAsync();
            decimal total = 0;
            foreach (var item in itens)
                total += item.Produto.Preco * item.Quantidade;

            return total;
        }
    }
}

