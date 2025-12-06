using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;
using System;
using RCL.Data.DTO.CarrinhoDTOs;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrinhoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<CarrinhoDTO>> ObterCarrinho(string userId)
        {
            var itens = await _context.CarrinhoItens
                .Where(c => c.UserId == userId)
                .Select(c => new CarrinhoItemDTO
                {
                    ProdutoId = c.ProdutoId,
                    Quantidade = c.Quantidade
                })
                .ToListAsync();

            return new CarrinhoDTO { Itens = itens };
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> AdicionarItem([FromBody] AddCarrinhoDTO req)
        {
            var existente = await _context.CarrinhoItens
                .FirstOrDefaultAsync(c => c.UserId == req.UserId && c.ProdutoId == req.ProdutoId);

            if (existente != null)
                existente.Quantidade += req.Quantidade;
            else
                _context.CarrinhoItens.Add(new CarrinhoItem
                {
                    UserId = req.UserId,
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
                .FirstOrDefaultAsync(c => c.UserId == req.UserId && c.ProdutoId == req.ProdutoId);

            if (item == null) return NotFound();

            if (req.Quantidade <= 0)
                _context.CarrinhoItens.Remove(item);
            else
                item.Quantidade = req.Quantidade;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{userId}/{produtoId}")]
        public async Task<IActionResult> Remover(string userId, int produtoId)
        {
            var item = await _context.CarrinhoItens
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProdutoId == produtoId);

            if (item != null)
                _context.CarrinhoItens.Remove(item);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("finalizar/{userId}")]
        public async Task<IActionResult> Finalizar(string userId)
        {
            var itens = _context.CarrinhoItens.Where(c => c.UserId == userId);

            _context.CarrinhoItens.RemoveRange(itens);
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}

        
