using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyColl.RCL.Data.Model;

public enum EstadoUtilizador
{
    Pendente,
    Activo,
    Inativo
}

public abstract class Utilizador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public EstadoUtilizador Estado { get; set; } = EstadoUtilizador.Pendente;

    // Relacionamentos comuns
    public DateTime DataRegisto { get; set; } = DateTime.UtcNow;
}
