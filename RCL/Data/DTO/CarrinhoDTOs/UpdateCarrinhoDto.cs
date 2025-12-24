namespace RCL.Data.DTO.CarrinhoDTOs
{
    public class UpdateCarrinhoDTO
    {
        public string UserId { get; set; } = null!;
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
