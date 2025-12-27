using Microsoft.AspNetCore.Components.Forms;
using RCL.Data.Model;
using RCL.Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IProdutoService
    {
        Task<Produto> InserirProdutoAsync(string fornecedorId, Produto produto, IBrowserFile? imagem);
        Task<List<Produto>> ListarProdutosAsync();
        Task<List<Produto>> ListarProdutosPorCategoriaAsync(int categoriaId);
        Task<Produto?> ObterProdutoDestaqueAsync();
        Task<List<Produto>> ConsultarProdutosAsync(string fornecedorId);
        Task<Produto?> EditarProdutoAsync(string fornecedorId, Produto produtoAtualizado);
        Task<bool> AlterarEstadoProdutoAsync(string fornecedorId, int produtoId, EstadoProduto novoEstado);
        Task<Produto?> ObterProdutoPorIdAsync(int produtoId);
    }
}
