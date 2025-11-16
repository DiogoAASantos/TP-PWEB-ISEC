using Microsoft.AspNetCore.Identity;
using RCL.Data.Model;

namespace API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public EstadoUtilizador Estado { get; set; } = EstadoUtilizador.Pendente;  
        public TipoUtilizador Tipo { get; set; } = TipoUtilizador.Cliente;        
        public DateTime DataRegisto { get; set; } = DateTime.UtcNow;              
    }
}
