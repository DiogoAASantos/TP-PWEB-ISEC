namespace RCL.Data.DTO
{
    public class CarrinhoItemDTO
    {
        public string Nome { get; set; } = null!;
        public decimal Preco { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
