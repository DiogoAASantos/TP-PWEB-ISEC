using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public class ClienteService : IClienteService
    {
        private readonly HttpClient _http;
        private readonly Cliente _cliente; // cliente atual (opcional, se quiseres manter carrinho local)

        public ClienteService(HttpClient http, Cliente cliente)
        {
            _http = http;
            _cliente = cliente;
        }

        // Efetivar compra: transforma o carrinho em encomenda
        public async Task<Encomenda> EfetivarCompraAsync()
        {
            if (_cliente.Carrinho == null || !_cliente.Carrinho.Any())
                throw new InvalidOperationException("Carrinho vazio.");

            var response = await _http.PostAsJsonAsync("/api/encomenda/efetivar", _cliente.Carrinho);
            response.EnsureSuccessStatusCode();

            var encomenda = await response.Content.ReadFromJsonAsync<Encomenda>();

            // Limpar carrinho local
            _cliente.Carrinho.Clear();

            return encomenda!;
        }

        // Consultar histórico de encomendas do cliente
        public async Task<List<Encomenda>> ConsultarHistoricoComprasAsync(int clienteId)
        {
            return await _http.GetFromJsonAsync<List<Encomenda>>("/api/clientes/historico")
           ?? new List<Encomenda>();
        }
    }
}
