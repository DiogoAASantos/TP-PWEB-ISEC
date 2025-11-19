using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IClienteService : IAnonimoService
    {
        // Logar como cliente
        Task<Cliente?> LoginAsync(string email, string password);

        // Efetivar compra (transforma o carrinho em encomenda)
        Task<Encomenda> EfetivarCompraAsync();

        // Consultar histórico de encomendas
        Task<List<Encomenda>> ConsultarHistoricoComprasAsync(int clienteId);
    }
}


