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
        public async Task<List<Produto>> ListarProdutosPorCategoriaAsync(   [FromQuery] string categoria,
                                                                            [FromQuery] string? subcategoria = null,
                                                                            [FromQuery] decimal? precoMin = null,
                                                                            [FromQuery] decimal? precoMax = null,
                                                                            [FromQuery] DisponibilidadeProduto? disponibilidade = null)
        {
            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria == categoria);

            if (!string.IsNullOrWhiteSpace(subcategoria))
                query = query.Where(p => p.SubCategoria == subcategoria);

            if (precoMin.HasValue)
                query = query.Where(p => p.Preco >= precoMin.Value);

            if (precoMax.HasValue)
                query = query.Where(p => p.Preco <= precoMax.Value);

            if (disponibilidade.HasValue)
                query = query.Where(p => p.Disponibilidade == disponibilidade.Value);

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
