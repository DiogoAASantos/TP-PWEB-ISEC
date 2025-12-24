using Blazored.LocalStorage;
using RCL.Data.DTO.Auth;
using RCL.Data.DTO.CarrinhoDTOs;
using RCL.Data.DTO.EncomendasDTOs;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public class ClienteService : IClienteService
    {
        private readonly HttpClient _http;
        private readonly IMyStorageService _localStorage;
        public List<CarrinhoItemDTO> Carrinho { get; private set; } = new();
        private UserDTO? _clienteLogado;

        public ClienteService(HttpClient http, IMyStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public void SetCliente(UserDTO cliente)
        {
            _clienteLogado = cliente;
        }

        public decimal ObterTotal()
        {
            return Carrinho.Sum(x => x.Preco * x.Quantidade);
        }

        public async Task<Cliente> RegistarComoClienteAsync(Cliente novoCliente)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/clientes/registar");
            request.Content = JsonContent.Create(novoCliente);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Cliente>() ?? novoCliente;
        }

        public async Task<List<Encomenda>> ConsultarHistoricoComprasAsync(string clienteId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/clientes/historico");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Encomenda>>() ?? new List<Encomenda>();
            }

            return new List<Encomenda>();
        }
    }
}
