namespace RCL.Data.DTO.CarrinhoDTOs
{
    public class CarrinhoItemDTO
    {
        public string Nome { get; set; } = null!;
        public decimal Preco { get; set; }
        public string ProdutoId { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}
