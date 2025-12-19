using RCL.Data.DTO.Auth;
using RCL.Data.DTO.CarrinhoDTOs;
using RCL.Data.DTO.EncomendasDTOs;
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
        public List<CarrinhoItemDTO> Carrinho { get; private set; } = new();
        private UserDTO? _clienteLogado;

        public ClienteService(HttpClient http)
        {
            _http = http;
        }

        public void SetCliente(UserDTO cliente)
        {
            _clienteLogado = cliente;
        }

        public decimal ObterTotal()
        {
            return Carrinho.Sum(x => x.Preco * x.Quantidade);
        }

        // Registar-se como cliente (estado Pendente)
        public async Task<Cliente> RegistarComoClienteAsync(Cliente novoCliente)
        {
            var response = await _http.PostAsJsonAsync("/api/clientes/registar", novoCliente);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Cliente>() ?? novoCliente;
        }

        // Consultar histórico de encomendas do cliente
        public async Task<List<Encomenda>> ConsultarHistoricoComprasAsync(string clienteId)
        {
            return await _http.GetFromJsonAsync<List<Encomenda>>("/api/clientes/historico")
           ?? new List<Encomenda>();
        }
    }
}
