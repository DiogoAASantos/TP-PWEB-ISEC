using System.ComponentModel.DataAnnotations.Schema;

namespace RCL.Data.Model
{
    public class CarrinhoItem
    {
        public int Id { get; set; }

        // Ligação ao Cliente (User Identity)
        public string ClienteId { get; set; } = string.Empty;

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        // Ligação ao Produto

        [ForeignKey("ProdutoId")]
        public Produto? Produto { get; set; }
        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }
    }
}