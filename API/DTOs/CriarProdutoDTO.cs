using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RCL.Data.DTO
{
    public class CriarProdutoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Stock { get; set; }
        public int Tipo { get; set; } 
        public int CategoriaId { get; set; }
        public string FornecedorId { get; set; }
        public int Estado { get; set; }
        public IFormFile? Imagem { get; set; }
    }
}
