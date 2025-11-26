using MyColl.RCL.Data.Model;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IFornecedorService
    {
        // Inserir novo produto (por padrão fica "Listado" até ser aprovado)
        Task<Produto> InserirProdutoAsync(int fornecedorId, Produto produto);

        // Consultar todos os produtos do fornecedor
        Task<List<Produto>> ConsultarProdutosAsync(int fornecedorId);

        // Editar detalhes do produto
        Task<Produto?> EditarProdutoAsync(int fornecedorId, Produto produtoAtualizado);

        // Alterar o estado do produto (Listar, A Venda, Vendido, etc.)
        Task<bool> AlterarEstadoProdutoAsync(int fornecedorId, int produtoId, EstadoProduto novoEstado);

        // Consultar histórico das vendas dos seus produtos
        Task<List<Encomenda>> ConsultarHistoricoVendasAsync(int fornecedorId);
    }
}


