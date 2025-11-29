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
    public class FornecedorService : IFornecedorService
    {
        private readonly HttpClient _http;

        public FornecedorService(HttpClient http)
        {
            _http = http;
        }

        private Fornecedor? _fornecedorLogado;

        public void SetCliente(UserDto fornecedor)
        {
            _fornecedorLogado = new Fornecedor
            {
                Id = fornecedor.Id,
                Email = fornecedor.Email,
                Nome = fornecedor.Nome
            };
        }

        // Consultar histórico de vendas
        public async Task<List<Encomenda>> ConsultarHistoricoVendasAsync(int fornecedorId)
        {
            // Chama a API para obter todas as encomendas que contenham produtos deste fornecedor
            return await _http.GetFromJsonAsync<List<Encomenda>>($"/api/fornecedores/{fornecedorId}/vendas")
                   ?? new List<Encomenda>();
        }
    }
}
