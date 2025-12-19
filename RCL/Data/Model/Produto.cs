using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCL.Data.Model.Enums;

namespace RCL.Data.Model
{
    public class Produto
    {
        public string Id { get; set; } = string.Empty;
        public string FornecedorId { get; set; } = string.Empty;
        
        [ForeignKey("FornecedorId")] 
        public Fornecedor? Fornecedor { get; set; }
        
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Stock { get; set; }
        public EstadoProduto Estado { get; set; }
        public DisponibilidadeProduto Disponibilidade { get; set; }
        public TipoProduto Tipo { get; set; }
        
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }


    }
}




