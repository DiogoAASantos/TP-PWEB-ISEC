using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public class Cliente : ApplicationUser
    {
        public List<Encomenda> HistoricoCompras { get; set; } = new();
        public List<Produto> Carrinho { get; set; } = new();
    }
}

