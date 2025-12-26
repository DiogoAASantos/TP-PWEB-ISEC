using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public class EncomendaItem
    {
        public int Id { get; set; } 
        public int EncomendaId { get; set; } 
        public Encomenda Encomenda { get; set; } = null!;
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
