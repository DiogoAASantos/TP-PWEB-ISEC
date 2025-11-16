using RCL.Data.Model;

namespace API.DTOs.Auth
{
    public class RegisterDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public TipoUtilizador Tipo { get; set; }
    }
}
