using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;
using RCL.Data.Model.Enums;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("disponiveis")]
        public async Task<List<Produto>> ListarProdutosAsync()
        {
            return await _context.Produtos
                .Where(p => p.Disponibilidade == DisponibilidadeProduto.EmStock)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<List<Produto>>> ListarProdutosPorCategoriaAsync(
                [FromQuery] int? categoriaId,        // MUDANÇA 1: Recebe o ID (int) em vez da Classe
                [FromQuery] int? subcategoriaId = null, // MUDANÇA 2: Subcategoria também é um ID
                [FromQuery] decimal? precoMin = null,
                [FromQuery] decimal? precoMax = null,
                [FromQuery] DisponibilidadeProduto? disponibilidade = null)
        {
            // Começa a consulta e carrega os dados relacionados
            var query = _context.Produtos
                .Include(p => p.Categoria) // Importante para ver o nome da categoria no JSON
                .AsQueryable();

            // Lógica de Filtro por Categoria (Corrigida)

            // 1. Se o utilizador escolheu uma subcategoria específica, filtramos por ela
            if (subcategoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaId == subcategoriaId.Value);
            }
            // 2. Se escolheu apenas a categoria principal, queremos produtos dela OU das filhas
            else if (categoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaId == categoriaId.Value
                                      || p.Categoria.CategoriaPaiId == categoriaId.Value);
            }

            // Filtros de Preço (Mantidos iguais aos teus)
            if (precoMin.HasValue)
            {
                query = query.Where(p => p.Preco >= precoMin.Value);
            }

            if (precoMax.HasValue)
            {
                query = query.Where(p => p.Preco <= precoMax.Value);
            }

            // Filtro de Disponibilidade (Mantido igual ao teu)
            if (disponibilidade.HasValue)
            {
                query = query.Where(p => p.Disponibilidade == disponibilidade.Value);
            }

            return await query.ToListAsync();
        }

        [HttpGet("destaque")]
        public async Task<Produto?> ObterProdutoDestaqueAsync()
        {
            return await _context.Produtos
                .Where(p => p.Disponibilidade == DisponibilidadeProduto.EmStock)
                .OrderBy(r => Guid.NewGuid()) // pega um aleatório
                .FirstOrDefaultAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> ObterProdutoPorId(int id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound(); 

            return Ok(produto);
        }
    }
}
