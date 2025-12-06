using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public enum EstadoEncomenda
    {
        Pendente,
        Confirmada,
        Rejeitada,
        Expedida,
    }

    public class Encomenda
    {
        public int Id { get; set; }
        public string ClienteId { get; set; } = string.Empty;

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        public DateTime Data_Encomenda { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public EstadoEncomenda Estado { get; set; } = EstadoEncomenda.Pendente;
        public List<EncomendaItem> Itens { get; set; } = new();
    }
}


