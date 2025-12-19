namespace RCL.Data.DTO.Auth
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Token { get; set; } = string.Empty;

    }
}
