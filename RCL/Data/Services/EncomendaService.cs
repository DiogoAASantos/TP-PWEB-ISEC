using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Services
{
    public class EncomendaService
    {
        private readonly HttpClient _http;

        public EncomendaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Encomenda> EfetivarCompraAsync()
        {
            // Envia um POST para a API para transformar o carrinho em encomenda
            var encomenda = await _http.PostAsJsonAsync("/api/encomendas/efetivar", new { });

            encomenda.EnsureSuccessStatusCode();

            // Lê o resultado como Encomenda
            return await encomenda.Content.ReadFromJsonAsync<Encomenda>()
                   ?? throw new Exception("Falha ao efetivar compra");
        }

        public async Task<List<Encomenda>> ConsultarHistoricoAsync(int clienteId)
        {
            var response = await _http.GetAsync($"/api/encomendas/{clienteId}");
            response.EnsureSuccessStatusCode();

            var historico = await response.Content.ReadFromJsonAsync<List<Encomenda>>();

            return historico ?? new List<Encomenda>();
        }
    }
}
