using RCL.Data.Model;
using RCL.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public class AnonimoService : IAnonimoService
    {
        private readonly List<Produto> _produtos; // Simula a base de dados de produtos
        private readonly Anonimo _usuario; // O anónimo atual com carrinho

        public AnonimoService(List<Produto> produtos, Anonimo usuario)
        {
            _produtos = produtos;
            _usuario = usuario;
        }

        // Listar todos os produtos disponíveis
        public async Task<List<Produto>> ListarProdutosAsync()
        {
            // Simula operação assíncrona
            return await Task.FromResult(_produtos.Where(p => p.Disponibilidade == DisponibilidadeProduto.EmStock).ToList());
        }

        // Listar produtos filtrando por categoria, subcategoria, preço e disponibilidade
        public async Task<List<Produto>> ListarProdutosPorCategoriaAsync(string categoria, string? subcategoria = null,
                                                                         decimal? precoMin = null,
                                                                         decimal? precoMax = null,
                                                                         DisponibilidadeProduto? disponibilidade = null)
        {
            var query = _produtos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(subcategoria))
                query = query.Where(p => p.SubCategoria != null && p.SubCategoria.Equals(subcategoria, StringComparison.OrdinalIgnoreCase));

            if (precoMin.HasValue)
                query = query.Where(p => p.Preco >= precoMin.Value);

            if (precoMax.HasValue)
                query = query.Where(p => p.Preco <= precoMax.Value);

            if (disponibilidade.HasValue)
                query = query.Where(p => p.Disponibilidade == disponibilidade);

            return await Task.FromResult(query.ToList());
        }

        // Obter um produto aleatório em destaque
        public async Task<Produto?> ObterProdutoDestaqueAsync()
        {
            var produtosDisponiveis = _produtos.Where(p => p.Disponibilidade == DisponibilidadeProduto.EmStock).ToList();
            if (!produtosDisponiveis.Any()) return null;

            var random = new Random();
            int index = random.Next(produtosDisponiveis.Count);
            return await Task.FromResult(produtosDisponiveis[index]);
        }

        // Adicionar produtos ao carrinho (simulação, sem efetivar compra)
        public async Task AddCarrinhoAsync(List<(int produtoId, int quantidade)> itens)
        {
            foreach (var (produtoId, quantidade) in itens)
            {
                var produto = _produtos.FirstOrDefault(p => p.Id == produtoId);
                if (produto != null)
                {
                    for (int i = 0; i < quantidade; i++)
                        _usuario.Carrinho.Add(produto);
                }
            }

            await Task.CompletedTask;
        }

        // Registar-se como cliente (estado Pendente)
        public async Task<Cliente> RegistarComoClienteAsync(Cliente novoCliente)
        {
            
        }
    }
}
