using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using RCL.Data.DTO.Auth;
using RCL.Data.DTO;

namespace RCL.Data.Interfaces
{
    public class FornecedorService : IFornecedorService
    {
        private readonly HttpClient _http;
        private UserDTO? _fornecedorLogado;

        public FornecedorService(HttpClient http)
        {
            _http = http;
        }

        public void SetFornecedor(UserDTO fornecedor)
        {
            _fornecedorLogado = fornecedor;
        }

        // Consultar histórico de vendas
        public async Task<List<VendaFornecedorDTO>> ObterVendasDoFornecedorAsync(string fornecedorId)
        {
            // Chama a API para obter todas as encomendas que contenham produtos deste fornecedor
            return await _http.GetFromJsonAsync<List<VendaFornecedorDTO>>($"/api/fornecedores/{fornecedorId}/vendas")
                   ?? new List<VendaFornecedorDTO>();
        }
    }
}
