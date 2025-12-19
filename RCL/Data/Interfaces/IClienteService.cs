using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCL.Data.DTO.Auth;

namespace RCL.Data.Interfaces
{
    public interface IClienteService
    {
        void SetCliente(UserDTO cliente);
        Task<Cliente> RegistarComoClienteAsync(Cliente novoCliente);
        Task<List<Encomenda>> ConsultarHistoricoComprasAsync(string clienteId);
    }
}


