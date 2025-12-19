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

        // --- Auto-Relacionamento ---

        // FK para o Pai (pode ser null se for uma categoria de topo, ex: "Moedas")
        public int? CategoriaPaiId { get; set; }

        // Navegação para o Pai
        [ForeignKey("CategoriaPaiId")]
        [JsonIgnore] // Evita ciclo infinito ao serializar
        public Categoria? CategoriaPai { get; set; }

        // Navegação para os Filhos (Subcategorias)
        // Ex: Se isto for "Moedas", a lista terá "Portugal", "Espanha", etc.
        public List<Categoria> SubCategorias { get; set; } = new();

        // --- Relação com Produtos ---
        // Um produto pertence a esta categoria
        [JsonIgnore]
        public List<Produto> Produtos { get; set; } = new();
    }
}