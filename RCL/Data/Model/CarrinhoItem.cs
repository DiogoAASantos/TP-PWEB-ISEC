using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public class CarrinhoItem
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }

        // Navegação
        public Produto Produto { get; set; } = null!;
    }

}
