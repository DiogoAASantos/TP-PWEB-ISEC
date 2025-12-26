using RCL.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDTO>> ObterCategoriasAsync();
    }
}
