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
        Task<List<Produto>> ListarProdutosAsync();

        Task<List<Produto>> ListarProdutosPorCategoriaAsync(int categoriaId);
        Task<Produto?> ObterProdutoDestaqueAsync();
        Task<Produto> InserirProdutoAsync(string fornecedorId, Produto produto);
        Task<List<Produto>> ConsultarProdutosAsync(string fornecedorId);
        Task<Produto?> EditarProdutoAsync(string fornecedorId, Produto produtoAtualizado);
        Task<bool> AlterarEstadoProdutoAsync(string fornecedorId, int produtoId, EstadoProduto novoEstado);
        Task<Produto?> ObterProdutoPorIdAsync(int produtoId);
    }
}
