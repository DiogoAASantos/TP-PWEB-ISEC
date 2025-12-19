namespace RCL.Data.DTO.CarrinhoDTOs
{
    public class UpdateCarrinhoDTO
    {
        public string UserId { get; set; } = null!;
        public string ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
