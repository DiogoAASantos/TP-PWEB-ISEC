using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public class Anonimo : Utilizador
    {
        // Carrinho temporário do visitante
        public List<Produto> Carrinho { get; set; } = new();
    }
}


