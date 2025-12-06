using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCL.Data.DTO.Auth;
using RCL.Data.DTO;

namespace RCL.Data.Interfaces
{
    public interface IFornecedorService
    {
        // Consultar histórico das vendas dos seus produtos
        Task<List<VendaFornecedorDTO>> ObterVendasDoFornecedorAsync(string fornecedorId);
        void SetFornecedor(UserDTO fornecedor);
    }
}


