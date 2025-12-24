using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.DTO.EncomendasDTOs
{
    public class EncomendaDTO
    {
        public int Id { get; set; }
        public DateTime Data_Encomenda { get; set; }
        public decimal Total { get; set; }
        public EstadoEncomenda Estado { get; set; } = EstadoEncomenda.Pendente;

        public List<EncomendaItemDTO> Itens { get; set; } = new();
    }
}
