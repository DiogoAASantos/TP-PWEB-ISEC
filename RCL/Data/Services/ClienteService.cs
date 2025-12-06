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

        public void AdicionarAoCarrinho(Produto produto, int quantidade)
        {
            var item = Carrinho.FirstOrDefault(x => x.ProdutoId == produto.Id);
            if (item != null)
            {
                item.Quantidade += quantidade;
            }
            else
            {
                Carrinho.Add(new CarrinhoItemDTO
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Quantidade = quantidade
                });
            }
        }

        public void RemoverDoCarrinho(int produtoId)
        {
            var item = Carrinho.FirstOrDefault(x => x.ProdutoId == produtoId);
            if (item != null) Carrinho.Remove(item);
        }

        public decimal ObterTotal()
        {
            return Carrinho.Sum(x => x.Preco * x.Quantidade);
        }

        // Efetivar compra: transforma o carrinho em encomenda
        public async Task<Encomenda> EfetivarCompraAsync()
        {
            if (_clienteLogado == null)
                throw new InvalidOperationException("Utilizador não autenticado.");

            if (!Carrinho.Any())
                throw new InvalidOperationException("Carrinho vazio.");

            // Cria o "Pacote" para a API (ID do Cliente + Lista de Itens)
            var encomendaDto = new CriarEncomendaDTO
            {
                ClienteId = _clienteLogado.Id, 
                Itens = Carrinho
            };

            var response = await _http.PostAsJsonAsync("/api/encomendas", encomendaDto);
            response.EnsureSuccessStatusCode();

            var encomendaCriada = await response.Content.ReadFromJsonAsync<Encomenda>();

            // Limpar carrinho local após sucesso
            Carrinho.Clear();

            return encomendaCriada!;
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
