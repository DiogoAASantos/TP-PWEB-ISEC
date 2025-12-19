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
        public List<EncomendaItemDTO> Itens { get; set; } = new();
    }
}
