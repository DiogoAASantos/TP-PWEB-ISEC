using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyColl.RCL.Data.Model;
public class Cliente : Utilizador
{
    public ICollection<Encomenda> Encomendas { get; set; } = new List<Encomenda>();
}
