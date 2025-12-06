using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public class Fornecedor : ApplicationUser
    {
        public string? Empresa { get; set; } = string.Empty;
        public List<Produto> Produtos{ get; set; } = new();
    }
}

