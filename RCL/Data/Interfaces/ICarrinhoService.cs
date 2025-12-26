using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCL.Data.Model;

namespace RCL.Data.Interfaces
{
    public interface ICarrinhoService
    {
        IReadOnlyList<CarrinhoItem> Carrinho { get; }
        Task<List<CarrinhoItem>> ObterItensAsync();
        Task LimparCarrinho();
        Task AdicionarProdutoAsync(int produtoId, int quantidade = 1);
        Task AtualizarQuantidadeAsync(int produtoId, int quantidade);
        Task RemoverProdutoAsync(int produtoId);
        Task<decimal> TotalAsync();
    }
}
