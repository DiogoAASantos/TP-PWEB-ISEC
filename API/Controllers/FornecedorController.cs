using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.DTO;
using RCL.Data.Model;
using RCL.Data.Model.Enums;
using System;

namespace API.Controllers
{
    [Authorize(Roles = "Fornecedor")]
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
            produto.Estado = EstadoProduto.AVenda;
            produto.Id = 0;

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ConsultarProduto), new { fornecedorId = produto.FornecedorId, produtoId = produto.Id }, produto);
        }

        [HttpGet("{fornecedorId}/produtos")]
        public async Task<ActionResult<List<Produto>>> ConsultarProdutos(string fornecedorId)
        {
            var produtos = await _context.Produtos
                .Where(p => p.FornecedorId == fornecedorId)
                .ToListAsync();

            return Ok(produtos);
        }

        // GET: api/fornecedores/{fornecedorId}/produtos/{produtoId}
        [HttpGet("{fornecedorId}/produtos/{produtoId}")]
        public async Task<ActionResult<Produto>> ConsultarProduto(string fornecedorId, int produtoId)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.FornecedorId == fornecedorId && p.Id == produtoId);

            if (produto == null) return NotFound();
            return Ok(produto);
        }

        // PUT: api/fornecedores/{fornecedorId}/produtos/{produtoId}
        [HttpPut("{fornecedorId}/produtos/{produtoId}")]
        public async Task<ActionResult<Produto?>> EditarProduto(string fornecedorId, int produtoId, [FromBody] Produto produtoAtualizado)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.FornecedorId == fornecedorId && p.Id == produtoId);

            if (produto == null) return NotFound();

            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.Preco = produtoAtualizado.Preco;
            produto.Categoria = produtoAtualizado.Categoria;
            //produto.SubCategoria = produtoAtualizado.SubCategoria;
            produto.Disponibilidade = produtoAtualizado.Disponibilidade;

            // Após edição, produto volta para pendente aprovação
            produto.Estado = EstadoProduto.PendenteAprovacao;

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        // PATCH: api/fornecedores/{fornecedorId}/produtos/{produtoId}/estado
        [HttpPatch("{fornecedorId}/produtos/{produtoId}/estado")]
        public async Task<ActionResult> AlterarEstadoProduto(string fornecedorId, int produtoId, [FromQuery] EstadoProduto novoEstado)
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
        public async Task<ActionResult<List<VendaFornecedorDTO>>> ConsultarHistoricoVendas(string fornecedorId)
        {
            var vendas = await _context.EncomendaItems
                .Include(ei => ei.Produto)
                .Include(ei => ei.Encomenda)
                .Where(ei => ei.Produto.FornecedorId == fornecedorId) 
                .OrderByDescending(ei => ei.Encomenda.Data_Encomenda)
                .Select(ei => new VendaFornecedorDTO
                {
                    DataVenda = ei.Encomenda.Data_Encomenda,
                    NomeProduto = ei.Produto.Nome,
                    Quantidade = ei.Quantidade,
                    PrecoUnitario = ei.PrecoUnitario,
                    Total = ei.Quantidade * ei.PrecoUnitario
                })
                .ToListAsync();

            return Ok(vendas);
        }
    }
}
