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

        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<List<Produto>>> ListarProdutosPorCategoriaAsync([FromRoute] int categoriaId)
        {
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId)
                .ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("destaque")]
        public async Task<Produto?> ObterProdutoDestaqueAsync()
        {
            return await _context.Produtos
                .Where(p => p.Disponibilidade == DisponibilidadeProduto.EmStock)
                .OrderBy(r => Guid.NewGuid()) 
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
