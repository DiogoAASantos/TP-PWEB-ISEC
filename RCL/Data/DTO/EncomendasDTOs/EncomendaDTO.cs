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
        public string Estado { get; set; } = string.Empty; // Envia o Enum como texto

        public List<EncomendaItemDTO> Itens { get; set; } = new();
    }
}
