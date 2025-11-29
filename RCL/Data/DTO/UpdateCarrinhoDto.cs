namespace RCL.Data.DTO
{
    public class UpdateCarrinhoDTO
    {
        public string UserId { get; set; } = null!;
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
