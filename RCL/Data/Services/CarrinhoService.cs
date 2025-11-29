using RCL.Data.DTO;
using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Services
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly HttpClient _http;
        private readonly string _userId;

        public CarrinhoService(HttpClient http, string userId)
        {
            _http = http;
            _userId = userId;
        }

        public async Task<List<CarrinhoItem>> ObterItensAsync()
        {
            var carrinhoDto = await _http.GetFromJsonAsync<CarrinhoDTO>($"api/carrinho/{_userId}");
            var itens = new List<CarrinhoItem>();

            if (carrinhoDto?.Itens != null)
            {
                foreach (var dto in carrinhoDto.Itens)
                {
                    // Aqui podes fazer outra chamada para obter Produto completo se necessário
                    itens.Add(new CarrinhoItem
                    {
                        UserId = _userId,
                        ProdutoId = dto.ProdutoId,
                        Quantidade = dto.Quantidade,
                        Produto = new Produto { Id = dto.ProdutoId, Nome = dto.Nome, Preco = dto.Preco } 
                    });
                }
            }

            return itens;
        }

        public async Task AdicionarProdutoAsync(int produtoId, int quantidade = 1)
        {
            var dto = new AddCarrinhoDTO
            {
                UserId = _userId,
                ProdutoId = produtoId,
                Quantidade = quantidade
            };

            await _http.PostAsJsonAsync("api/carrinho/adicionar", dto);
        }

        public async Task AtualizarQuantidadeAsync(int produtoId, int quantidade)
        {
            var dto = new UpdateCarrinhoDTO
            {
                UserId = _userId,
                ProdutoId = produtoId,
                Quantidade = quantidade
            };

            await _http.PutAsJsonAsync("api/carrinho/atualizar", dto);
        }

        public async Task RemoverProdutoAsync(int produtoId)
        {
            await _http.DeleteAsync($"api/carrinho/{_userId}/{produtoId}");
        }

        public async Task<bool> FinalizarCompraAsync()
        {
            var response = await _http.PostAsync($"api/carrinho/finalizar/{_userId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task LimparCarrinhoAsync()
        {
            var response = await _http.PostAsync($"api/carrinho/finalizar/{_userId}", null);
            response.EnsureSuccessStatusCode();
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

