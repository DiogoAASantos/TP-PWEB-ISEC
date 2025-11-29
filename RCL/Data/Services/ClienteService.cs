using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using RCL.Data.DTO.Auth;

namespace RCL.Data.Interfaces
{
    public class ClienteService : IClienteService
    {
        private readonly HttpClient _http;
        
        public ClienteService(HttpClient http)
        {
            _http = http;
        }

        private Cliente? _clienteLogado;

        public void SetCliente(UserDTO cliente)
        {
            _clienteLogado = new Cliente
            {
                Id = cliente.Id,
                Email = cliente.Email,
                Nome = cliente.Nome
                // Carrinho inicial vazio
            };
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

        // Registar-se como cliente (estado Pendente)
        public async Task<Cliente> RegistarComoClienteAsync(Cliente novoCliente)
        {
            var response = await _http.PostAsJsonAsync("/api/clientes/registar", novoCliente);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Cliente>() ?? novoCliente;
        }

        // Consultar histórico de encomendas do cliente
        public async Task<List<Encomenda>> ConsultarHistoricoComprasAsync(int clienteId)
        {
            return await _http.GetFromJsonAsync<List<Encomenda>>("/api/clientes/historico")
           ?? new List<Encomenda>();
        }
    }
}
