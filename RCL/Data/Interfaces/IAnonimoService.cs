using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IAnonimoService
    {
        // Listar produtos disponíveis
        Task<List<Produto>> ListarProdutosAsync();

        // Listar produtos por categoria/subcategoria
        Task<List<Produto>> ListarProdutosPorCategoriaAsync(string categoria, string? subcategoria = null, 
                                                            decimal? precoMin = null,
                                                            decimal? precoMax = null,
                                                            DisponibilidadeProduto? disponivel = null);

        // Obter um produto em destaque (aleatório)
        Task<Produto?> ObterProdutoDestaqueAsync();

        // Selecionar produtos e quantidades para compra (simulação, sem efetivar)
        Task AddCarrinhoAsync(List<(int produtoId, int quantidade)> itens);

        // Registar-se como cliente (estado Pendente)
        Task<Cliente> RegistarComoClienteAsync(Cliente novoCliente);
    }
}


