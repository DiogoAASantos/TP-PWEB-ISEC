using Blazored.LocalStorage;
using RCL.Data.DTO;
using RCL.Data.DTO.Auth;
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
    public class FornecedorService : IFornecedorService
    {
        private readonly HttpClient _http;
        private UserDTO? _fornecedorLogado;
        private readonly IMyStorageService _localStorage;

        public FornecedorService(HttpClient http, IMyStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public void SetFornecedor(UserDTO fornecedor)
        {
            _fornecedorLogado = fornecedor;
        }

        public async Task<List<VendaFornecedorDTO>> ObterVendasDoFornecedorAsync(string fornecedorId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/fornecedores/{fornecedorId}/vendas");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<VendaFornecedorDTO>>()
                       ?? new List<VendaFornecedorDTO>();
            }

            return new List<VendaFornecedorDTO>();
        }
    }
}
