using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.DTO;
using RCL.Data.DTO.EncomendasDTOs;
using RCL.Data.Model;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncomendasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EncomendasController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("efetivar")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<EncomendaDTO>> EfetivarCompra([FromBody] CriarEncomendaDTO dto)
        {
            var clienteIdDoToken = User.FindFirst("nameid")?.Value
                                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clienteIdDoToken))
                return Unauthorized("ID do cliente não encontrado.");

            if (dto.Itens == null || !dto.Itens.Any())
                return BadRequest("Carrinho vazio.");

            var novaEncomenda = new Encomenda
            {
                ClienteId = clienteIdDoToken,
                Data_Encomenda = DateTime.UtcNow,
                Estado = EstadoEncomenda.Pendente,
                Itens = new List<EncomendaItem>()
            };

            decimal totalCalculado = 0;


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var itemDto in dto.Itens)
                {
                    var produtoDb = await _context.Produtos.FindAsync(itemDto.ProdutoId);

                    if (produtoDb == null)
                        return BadRequest($"Produto com ID {itemDto.ProdutoId} não existe.");

                    if (produtoDb.Stock < itemDto.Quantidade)
                    {
                        return BadRequest($"Stock insuficiente para o produto '{produtoDb.Nome}'. Disponível: {produtoDb.Stock}.");
                    }

                    produtoDb.Stock -= itemDto.Quantidade;
                    
                    var novoItem = new EncomendaItem
                    {
                        ProdutoId = produtoDb.Id,
                        Quantidade = itemDto.Quantidade,
                        PrecoUnitario = produtoDb.Preco
                    };

                    novaEncomenda.Itens.Add(novoItem);
                    totalCalculado += (produtoDb.Preco * itemDto.Quantidade);

                    _context.Entry(produtoDb).State = EntityState.Modified;
                }

                novaEncomenda.Total = totalCalculado;

                _context.Encomendas.Add(novaEncomenda);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync(); 
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); 
                return StatusCode(500, "Erro ao processar encomenda: " + ex.Message);
            }

            var resultadoDto = new EncomendaDTO
            {
                Id = novaEncomenda.Id,
                Data_Encomenda = novaEncomenda.Data_Encomenda,
                Total = novaEncomenda.Total,
                Estado = novaEncomenda.Estado
            };

            return Ok(resultadoDto);
        }

        [HttpGet("historico")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<List<EncomendaDTO>>> ObterHistorico()
        {
            var clienteIdDoToken = User.FindFirst("nameid")?.Value
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clienteIdDoToken))
                return Unauthorized("ID do cliente não encontrado no Token.");

            var encomendasDb = await _context.Encomendas
                .Include(e => e.Itens)      
                .ThenInclude(i => i.Produto)
                .Where(e => e.ClienteId == clienteIdDoToken)
                .OrderByDescending(e => e.Data_Encomenda)
                .ToListAsync();

            if (encomendasDb == null || !encomendasDb.Any())
                return Ok(new List<EncomendaDTO>());

            var listaDto = encomendasDb.Select(e => new EncomendaDTO
            {
                Id = e.Id,
                Data_Encomenda = e.Data_Encomenda,
                Total = e.Total,
                Estado = e.Estado, 
                Itens = e.Itens.Select(i => new EncomendaItemDTO
                {
                    NomeProduto = i.Produto.Nome,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            }).ToList();

            return Ok(listaDto);
        }        
    }
}
