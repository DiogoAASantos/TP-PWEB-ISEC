using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public enum EstadoProduto
    {
        AVenda,
        Vendido,
        ParaAlugar,
        PendenteAprovacao
    }

    public enum DisponibilidadeProduto
    {
        EmStock,
        Esgotado
    }

    public enum TipoProduto
    {
        Colecionavel,
        Complemento
    }

    public class Produto
    {
        public int Id { get; set; }
        public int FornecedorId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Stock { get; set; }
        public EstadoProduto Estado { get; set; }
        public DisponibilidadeProduto Disponibilidade { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public Categoria Categoria { get; set; } = new Categoria();


    }
}




