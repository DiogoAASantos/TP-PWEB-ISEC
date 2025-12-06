using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.DTO.EncomendasDTOs
{
    public class EncomendaItemDTO
    {
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        // Calculado: Preco * Quantidade
        public decimal Subtotal => PrecoUnitario * Quantidade;
    }
}
