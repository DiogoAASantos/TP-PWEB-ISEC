namespace RCL.Data.DTO.CarrinhoDTOs
{
    public class AddCarrinhoDTO
    {
        public string UserId { get; set; } = null!;
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
