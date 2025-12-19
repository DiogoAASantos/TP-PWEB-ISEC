using RCL.Data.DTO;
using RCL.Data.DTO.EncomendasDTOs;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IEncomendaService
    {
        // Efetivar compra (transforma o carrinho em encomenda)
        Task<Encomenda> EfetivarCompraAsync();

        // Consultar histórico de encomendas
        Task<List<EncomendaDTO>> ConsultarHistoricoAsync();
        Task<List<VendaFornecedorDTO>> ObterVendasDoFornecedorAsync();
    }
}
