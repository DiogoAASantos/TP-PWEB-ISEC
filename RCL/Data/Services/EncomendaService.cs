using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RCL.Data.DTO;
using RCL.Data.DTO.EncomendasDTOs;
using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Services
{
    public class EncomendaService : IEncomendaService
    {
        private readonly HttpClient _http;
        private readonly ICarrinhoService _carrinhoService;
        private readonly IMyStorageService _localStorage;

        public EncomendaService(HttpClient http, ICarrinhoService carrinhoService, IMyStorageService localStorage)
        {
            _http = http;
            _carrinhoService = carrinhoService;
            _localStorage = localStorage;
        }

        private async Task<HttpRequestMessage> CreateAuthenticatedRequestAsync(HttpMethod method, string uri, object? content = null)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var request = new HttpRequestMessage(method, uri);

            if (content != null)
                request.Content = JsonContent.Create(content);

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));

            return request;
        }

        public async Task<Encomenda> EfetivarCompraAsync()
        {
            var itensCarrinho = await _carrinhoService.ObterItensAsync();

            if (itensCarrinho == null || !itensCarrinho.Any())
                throw new InvalidOperationException("Carrinho vazio. Adicione produtos antes de finalizar a compra.");

            var encomendaDto = new CriarEncomendaDTO
            {
                Itens = itensCarrinho.Select(item => new EncomendaItemDTO
                {
                    ProdutoId = item.ProdutoId,
                    NomeProduto = item.Produto.Nome,
                    PrecoUnitario = item.Produto.Preco,
                    Quantidade = item.Quantidade
                }).ToList()
            };

            var request = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/api/encomendas/efetivar", encomendaDto);
            var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            _carrinhoService.LimparCarrinhoLocal();

            return await response.Content.ReadFromJsonAsync<Encomenda>()
                   ?? throw new Exception("Falha ao ler resposta da encomenda.");
        }

        public async Task<List<EncomendaDTO>> ConsultarHistoricoAsync()
        {
            var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, "api/encomendas/historico");
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<EncomendaDTO>>() ?? new();
            }
            return new();
        }

        public async Task<List<VendaFornecedorDTO>> ObterVendasDoFornecedorAsync()
        {
            var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, "api/encomendas/fornecedor/vendas");
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<VendaFornecedorDTO>>() ?? new();
            }
            return new();
        }
    }
}
