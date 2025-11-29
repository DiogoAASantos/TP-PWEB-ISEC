using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.DTO.Auth
{
    public class RegisterDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Morada { get; set; } = "";
        public string Telefone { get; set; } = "";
        public TipoUtilizador TipoUtilizador { get; set; }
        public string? Empresa { get; set; }
    }
}
