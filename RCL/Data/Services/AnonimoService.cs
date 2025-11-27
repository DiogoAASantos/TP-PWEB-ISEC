using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace RCL.Data.Model
{
    public class AnonimoService : IAnonimoService
    {
        private readonly HttpClient _http;

        public AnonimoService(HttpClient http)
        {
            _http = http;
        }

        // Adicionar produtos ao carrinho (simulação, sem efetivar compra)
        // Agora o carrinho pode ser apenas mantido no frontend (ex: sessão, local storage ou objeto em memória)
        public async Task AddCarrinhoAsync(List<(int produtoId, int quantidade)> itens)
        {
            // Enviar para API ou apenas atualizar carrinho local (dependendo da arquitetura)
            // Aqui vamos assumir que é local:
            var response = await _http.PostAsJsonAsync("/api/carrinho/adicionar", itens);
            response.EnsureSuccessStatusCode();
        }

        // Registar-se como cliente (estado Pendente)
        public async Task<Cliente> RegistarComoClienteAsync(Cliente novoCliente)
        {
            var response = await _http.PostAsJsonAsync("/api/clientes/registar", novoCliente);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Cliente>() ?? novoCliente;
        }
    }
}

