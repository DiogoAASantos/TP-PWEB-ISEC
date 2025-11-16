using RCL.Data.Model;

namespace MyColl.RCL.Data.Interfaces
{
    public interface IFuncionarioService
    {
        // Gestão de Produtos
        IEnumerable<Produto> ListarProdutos(Categoria categoria, DisponibilidadeProduto disponibilidade);
        Task<Produto> AdicionarProdutoAsync(Produto produto);
        Task<Produto?> EditarProdutoAsync(Produto produto);
        Task<bool> ApagarProdutoAsync(int produtoId);
        Task<bool> AtivarProdutoAsync(int produtoId);
        Task<bool> InativarProdutoAsync(int produtoId);
        Task<bool> AtualizarPrecoEStockAsync(int produtoId, decimal novoPreco, int novoStock);
        Task<bool> AlterarEstadoProdutoAsync(int produtoId, EstadoProduto novoEstado);

        // Gestão de Clientes e Fornecedores
        Task<bool> AtivarUtilizadorAsync(int utilizadorId);
        Task<bool> InativarUtilizadorAsync(int utilizadorId);


        // Gestão de Vendas
        Task<bool> ConfirmarVendaAsync(int encomendaId);
        Task<bool> RejeitarVendaAsync(int encomendaId);
        Task<bool> ExpedirProdutoAsync(int encomendaId);
        Task<bool> AtualizarStockAposVendaAsync(int produtoId, int quantidadeVendida);
    }
}
