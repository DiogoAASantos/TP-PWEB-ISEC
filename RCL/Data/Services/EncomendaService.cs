using Microsoft.AspNetCore.Components.Authorization;
using RCL.Data.DTO;
using RCL.Data.DTO.EncomendasDTOs;
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
    public class EncomendaService : IEncomendaService
    {
        private readonly HttpClient _http;
        private readonly ICarrinhoService _carrinhoService; 

        public EncomendaService(HttpClient http, ICarrinhoService carrinhoService)
        {
            _http = http;
            _carrinhoService = carrinhoService;
        }

        public async Task<Encomenda> EfetivarCompraAsync()
        {
            if (!_carrinhoService.Carrinho.Any()) // Assumindo que Carrinho está acessível/método no ICarrinhoService
                throw new InvalidOperationException("Carrinho vazio. Adicione produtos antes de finalizar a compra.");

            // 2. CRIA O DTO SEGURO (Sem enviar o ClienteId, pois a API o obtém do Token)
            var encomendaDto = new CriarEncomendaDTO
            {
                Itens = _carrinhoService.Carrinho.Select(item => new EncomendaItemDTO
                {
                    NomeProduto = item.Produto.Nome,
                    PrecoUnitario = item.Produto.Preco,
                    ProdutoId = item.Produto.Id,
                    Quantidade = item.Quantidade
                }).ToList()
            };

            // 3. CHAMA A API SEGURA
            var response = await _http.PostAsJsonAsync("/api/encomendas/efetivar", encomendaDto); 

            response.EnsureSuccessStatusCode();

            // 4. LIMPEZA LOCAL (SÓ APÓS O SUCESSO DA API)
            _carrinhoService.LimparCarrinhoLocal(); // Assumindo que este método existe no CarrinhoService

            // 5. Devolve o resultado
            return await response.Content.ReadFromJsonAsync<Encomenda>()
                        ?? throw new Exception("Falha ao efetivar compra e obter encomenda.");
        }

        public async Task<List<EncomendaDTO>> ConsultarHistoricoAsync()
        {
            var response = await _http.GetAsync($"api/encomendas/historico");
            response.EnsureSuccessStatusCode();

            var historico = await response.Content.ReadFromJsonAsync<List<EncomendaDTO>>();

            return historico ?? new List<EncomendaDTO>();
        }

        public async Task<List<VendaFornecedorDTO>> ObterVendasDoFornecedorAsync()
        {
            return await _http.GetFromJsonAsync<List<VendaFornecedorDTO>>($"api/encomendas/fornecedor/vendas")
                   ?? new List<VendaFornecedorDTO>();
        }
    }
}
