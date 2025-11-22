using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncomendaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EncomendaController(ApplicationDbContext context)
        {
            _context = context;
        }


        // POST: api/clientes/efetivar
        [HttpPost("efetivar")]
        public async Task<ActionResult<Encomenda>> EfetivarCompra([FromBody] List<Produto> carrinho)
        {
            if (carrinho == null || !carrinho.Any())
                return BadRequest("Carrinho vazio.");

            // Criar Encomenda
            var encomendaItens = carrinho
                .GroupBy(p => p.Id)
                .Select(g => new EncomendaItem
                {
                    ProdutoId = g.Key,
                    Quantidade = g.Count(),
                    PrecoUnitario = g.First().Preco
                })
                .ToList();

            var encomenda = new Encomenda
            {
                Id_Cliente = 0, // Aqui você precisaria do cliente logado, exemplo: via JWT
                Data_Encomenda = DateTime.UtcNow,
                Itens = encomendaItens,
                Total = encomendaItens.Sum(i => i.Quantidade * i.PrecoUnitario),
                Estado = EstadoEncomenda.Pendente
            };

            _context.Encomendas.Add(encomenda);
            await _context.SaveChangesAsync();

            return Ok(encomenda);
        }
    }
}
