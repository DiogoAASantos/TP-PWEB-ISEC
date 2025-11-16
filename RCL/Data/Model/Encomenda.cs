using RCL.Data.Model;
using System;
using System.Collections.Generic;
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
        public int Id_Cliente { get; set; }
        public DateTime Data_Encomenda { get; set; } = DateTime.UtcNow;
        public decimal Total;
        public EstadoEncomenda Estado { get; set; } = EstadoEncomenda.Pendente;
        public List<EncomendaItem> itens { get; set; } = new();
    }
}


