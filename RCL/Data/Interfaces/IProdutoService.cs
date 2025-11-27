using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IProdutoService
    {
        // Listar todos os produtos disponíveis
        Task<List<Produto>> ListarProdutosAsync();

        // Listar produtos por filtros
        Task<List<Produto>> ListarProdutosPorCategoriaAsync(
            string categoria,
            string? subcategoria = null,
            decimal? precoMin = null,
            decimal? precoMax = null,
            DisponibilidadeProduto? disponibilidade = null
        );

        // Obter um produto em destaque
        Task<Produto?> ObterProdutoDestaqueAsync();

        // Inserir novo produto (fornecedor)
        Task<Produto> InserirProdutoAsync(int fornecedorId, Produto produto);

        // Consultar produtos do fornecedor
        Task<List<Produto>> ConsultarProdutosAsync(int fornecedorId);

        // Editar produto
        Task<Produto?> EditarProdutoAsync(int fornecedorId, Produto produtoAtualizado);

        // Alterar estado do produto (aprovado, pendente, rejeitado)
        Task<bool> AlterarEstadoProdutoAsync(int fornecedorId, int produtoId, EstadoProduto novoEstado);
        Task<Produto?> ObterProdutoPorIdAsync(int produtoId);
    }
}
