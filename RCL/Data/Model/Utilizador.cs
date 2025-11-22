using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Model
{
    public enum EstadoUtilizador
    {
        Pendente,
        Ativo,
        Inativo
    }

    public enum TipoUtilizador
    {
        Administrador,
        Funcionario,
        Cliente,
        Fornecedor
    }

    public abstract class Utilizador
    {
        public int Id { get; set; }  // PK para EF
        public string Nome { get; set; } = string.Empty;
        public EstadoUtilizador Estado { get; set; } = EstadoUtilizador.Pendente;

        // Atributos comuns a Cliente e Fornecedor
        public string Morada { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public bool EstaPendente { get; set; } = true;
    }
}


