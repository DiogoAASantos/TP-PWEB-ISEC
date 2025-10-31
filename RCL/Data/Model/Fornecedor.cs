using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyColl.RCL.Data.Model;
public class Fornecedor : Utilizador
{
    public string NomeEmpresa { get; set; } = string.Empty;
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
