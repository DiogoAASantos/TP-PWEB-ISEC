using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Importante para evitar ciclos no JSON

namespace RCL.Data.Model
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public int? CategoriaPaiId { get; set; }
        public string? ImagemUrl { get; set; } 

        [ForeignKey("CategoriaPaiId")]
        [JsonIgnore] 
        public Categoria? CategoriaPai { get; set; }

        public List<Categoria> SubCategorias { get; set; } = new();

        [JsonIgnore]
        public List<Produto> Produtos { get; set; } = new();


    }
}