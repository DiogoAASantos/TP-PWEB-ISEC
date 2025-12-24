using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.DTO.CarrinhoDTOs;
using RCL.Data.Model;
using System;
using System.Security.Claims;


namespace API.Controllers
{
    [Authorize(Roles = "Cliente")]
    [ApiController]
    [Route("api/[controller]")]
    public class CarrinhoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<CarrinhoDTO>> ObterCarrinho()
        {
            var userIdDoToken = User.FindFirst("nameid")?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var itens = await _context.CarrinhoItens
                .Include(c => c.Produto) 
                .Where(c => c.ClienteId == userIdDoToken)
                .Select(c => new CarrinhoItemDTO
                {
                    ProdutoId = c.ProdutoId,
                    Quantidade = c.Quantidade,
                    Nome = c.Produto.Nome, 
                    Preco = c.Produto.Preco  
                })
                .ToListAsync();

            return new CarrinhoDTO { Itens = itens };
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> AdicionarItem([FromBody] AddCarrinhoDTO req)
        {
            var existente = await _context.CarrinhoItens
                .FirstOrDefaultAsync(c => c.ClienteId == req.UserId && c.ProdutoId == req.ProdutoId);

            if (existente != null)
                existente.Quantidade += req.Quantidade;
            else
                _context.CarrinhoItens.Add(new CarrinhoItem
                {
                    ClienteId = req.UserId,
                    ProdutoId = req.ProdutoId,
                    Quantidade = req.Quantidade
                });

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("atualizar")]
        public async Task<IActionResult> AtualizarQuantidade([FromBody] UpdateCarrinhoDTO req)
        {
            var item = await _context.CarrinhoItens
                .FirstOrDefaultAsync(c => c.ClienteId == req.UserId && c.ProdutoId == req.ProdutoId);

            if (item == null) return NotFound();

            if (req.Quantidade <= 0)
                _context.CarrinhoItens.Remove(item);
            else
                item.Quantidade = req.Quantidade;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{produtoId}")]
        public async Task<IActionResult> Remover(int produtoId)
        {
            var clienteId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clienteId))
            {
                return Unauthorized("O ID do utilizador não foi encontrado no token.");
            }

            var item = await _context.CarrinhoItens
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId && c.ProdutoId == produtoId); // Usa clienteId em vez de userId

            if (item != null)
                _context.CarrinhoItens.Remove(item);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

        
