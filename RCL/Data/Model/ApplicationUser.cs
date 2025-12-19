using Microsoft.AspNetCore.Identity;
using RCL.Data.Model.Enums;

namespace RCL.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nome { get; set; } 

        public EstadoUtilizador Estado { get; set; } = EstadoUtilizador.Pendente;

        public TipoUtilizador Tipo { get; set; } // Para facilitar queries

        // Data de registo é útil para auditoria
        public DateTime DataRegisto { get; set; } = DateTime.UtcNow;
    }
}
