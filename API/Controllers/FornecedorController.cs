using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RCL.Data.Model;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FornecedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/fornecedores/produtos
        [HttpPost("produtos")]
        public async Task<ActionResult<Produto>> InserirProduto([FromBody] Produto produto)
        {
            produto.Estado = EstadoProduto.PendenteAprovacao;
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ConsultarProduto), new { fornecedorId = produto.FornecedorId, produtoId = produto.Id }, produto);
        }

        [HttpGet("{fornecedorId}/produtos")]
        [Authorize(Roles = "Fornecedor")]
        public async Task<ActionResult<List<Produto>>> ConsultarProdutos(int fornecedorId)
        {
            var produtos = await _context.Produtos
                .Where(p => p.FornecedorId == fornecedorId)
                .ToListAsync();
            return Ok(produtos);
        }

        // GET: api/fornecedores/{fornecedorId}/produtos/{produtoId}
        [HttpGet("{fornecedorId}/produtos/{produtoId}")]
        public async Task<ActionResult<Produto>> ConsultarProduto(int fornecedorId, int produtoId)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.FornecedorId == fornecedorId && p.Id == produtoId);

            if (produto == null) return NotFound();
            return Ok(produto);
        }

        // PUT: api/fornecedores/{fornecedorId}/produtos/{produtoId}
        [HttpPut("{fornecedorId}/produtos/{produtoId}")]
        public async Task<ActionResult<Produto?>> EditarProduto(int fornecedorId, int produtoId, [FromBody] Produto produtoAtualizado)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.FornecedorId == fornecedorId && p.Id == produtoId);

            if (produto == null) return NotFound();

            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.PrecoBase = produtoAtualizado.PrecoBase;
            produto.Categoria = produtoAtualizado.Categoria;
            produto.SubCategoria = produtoAtualizado.SubCategoria;
            produto.Disponibilidade = produtoAtualizado.Disponibilidade;

            // Após edição, produto volta para pendente aprovação
            produto.Estado = EstadoProduto.PendenteAprovacao;

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        // PATCH: api/fornecedores/{fornecedorId}/produtos/{produtoId}/estado
        [HttpPatch("{fornecedorId}/produtos/{produtoId}/estado")]
        public async Task<ActionResult> AlterarEstadoProduto(int fornecedorId, int produtoId, [FromQuery] EstadoProduto novoEstado)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.FornecedorId == fornecedorId && p.Id == produtoId);

            if (produto == null) return NotFound();

            produto.Estado = novoEstado;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/fornecedores/{fornecedorId}/vendas
        [HttpGet("{fornecedorId}/vendas")]
        public async Task<ActionResult<List<Encomenda>>> ConsultarHistoricoVendas(int fornecedorId)
        {
            // Todas as encomendas que contenham produtos deste fornecedor
            var encomendas = await _context.Encomendas
                .Include(e => e.Itens)
                .ThenInclude(i => i.Produto)
                .Where(e => e.Itens.Any(i => i.Produto.FornecedorId == fornecedorId))
                .ToListAsync();

            return Ok(encomendas);
        }
    }
}
