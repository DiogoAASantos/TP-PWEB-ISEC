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
        void LimparCarrinhoLocal();
        Task AdicionarProdutoAsync(string produtoId, int quantidade = 1);
        Task AtualizarQuantidadeAsync(string produtoId, int quantidade);
        Task RemoverProdutoAsync(string produtoId);
        Task<decimal> TotalAsync();
    }
}
