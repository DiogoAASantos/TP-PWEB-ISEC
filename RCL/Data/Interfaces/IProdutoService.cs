using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCL.Data.Model.Enums;

namespace RCL.Data.Interfaces
{
    public interface IProdutoService
    {
        // Listar todos os produtos disponíveis
        Task<List<Produto>> ListarProdutosAsync();

        // Listar produtos por filtros
        Task<List<Produto>> ListarProdutosPorCategoriaAsync(
            int categoriaId,
            int? subcategoriaId = null,
            decimal? precoMin = null,
            decimal? precoMax = null,
            DisponibilidadeProduto? disponibilidade = null
        );

        // Obter um produto em destaque
        Task<Produto?> ObterProdutoDestaqueAsync();

        // Inserir novo produto (fornecedor)
        Task<Produto> InserirProdutoAsync(string fornecedorId, Produto produto);

        // Consultar produtos do fornecedor
        Task<List<Produto>> ConsultarProdutosAsync(string fornecedorId);

        // Editar produto
        Task<Produto?> EditarProdutoAsync(string fornecedorId, Produto produtoAtualizado);

        // Alterar estado do produto (aprovado, pendente, rejeitado)
        Task<bool> AlterarEstadoProdutoAsync(string fornecedorId, string produtoId, EstadoProduto novoEstado);
        Task<Produto?> ObterProdutoPorIdAsync(string produtoId);
    }
}
