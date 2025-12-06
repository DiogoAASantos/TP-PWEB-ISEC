using RCL.Data.DTO.CarrinhoDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.DTO.EncomendasDTOs
{
    public class CriarEncomendaDTO
    {
        public string ClienteId { get; set; } = string.Empty;
        public List<CarrinhoItemDTO> Itens { get; set; } = new();
    }
}
