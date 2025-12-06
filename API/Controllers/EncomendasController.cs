using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.DTO;
using RCL.Data.DTO.EncomendasDTOs;
using RCL.Data.Model;

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


        // POST: api/clientes/efetivar
        [HttpPost("efetivar")]
        public async Task<ActionResult<int>> EfetivarCompra([FromBody] CriarEncomendaDTO dto)
        {
            if (dto.Itens == null || !dto.Itens.Any())
                return BadRequest("Carrinho vazio.");

            if (string.IsNullOrEmpty(dto.ClienteId))
                return BadRequest("ID do cliente inválido.");

            // 1. Criar a nova Encomenda (esqueleto)
            var novaEncomenda = new Encomenda
            {
                ClienteId = dto.ClienteId, // String (Identity)
                Data_Encomenda = DateTime.UtcNow,
                Estado = EstadoEncomenda.Pendente,
                Itens = new List<EncomendaItem>()
            };

            decimal totalCalculado = 0;

            // 2. Processar Itens (Validar Preços no Servidor)
            foreach (var itemDto in dto.Itens)
            {
                // Vamos à BD buscar o produto REAL para garantir que o preço está certo
                var produtoDb = await _context.Produtos.FindAsync(itemDto.ProdutoId);

                if (produtoDb == null) continue; // Produto não existe, salta fora

                // Calcula o preço com base no que está na BD (Segurança)
                decimal precoFinal = produtoDb.Preco;

                var novoItem = new EncomendaItem
                {
                    ProdutoId = produtoDb.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = precoFinal
                };

                novaEncomenda.Itens.Add(novoItem);
                totalCalculado += (precoFinal * itemDto.Quantidade);
            }

            novaEncomenda.Total = totalCalculado;

            // 3. Guardar na BD
            _context.Encomendas.Add(novaEncomenda);
            await _context.SaveChangesAsync();

            // Retorna o ID da encomenda criada ou o objeto completo mapeado
            return Ok(new { EncomendaId = novaEncomenda.Id });
        }

        // GET: api/encomendas/{clienteId}
        [HttpGet("{clienteId}")]
        public async Task<ActionResult<List<EncomendaDTO>>> ObterHistorico(string clienteId)
        {
            // 1. Buscar dados à BD
            var encomendasDb = await _context.Encomendas
                .Include(e => e.Itens)      // Atenção: Usei 'Itens' (Maiúscula)
                .ThenInclude(i => i.Produto)
                .Where(e => e.ClienteId == clienteId) // Comparação de strings
                .OrderByDescending(e => e.Data_Encomenda)
                .ToListAsync();

            if (encomendasDb == null || !encomendasDb.Any())
                return Ok(new List<EncomendaDTO>());

            // 2. Mapear Entidade -> DTO (Para evitar JSON Cycle e enviar dados limpos)
            var listaDto = encomendasDb.Select(e => new EncomendaDTO
            {
                Id = e.Id,
                Data_Encomenda = e.Data_Encomenda,
                Total = e.Total,
                Estado = e.Estado.ToString(), // Envia "Pendente" em vez de 0
                Itens = e.Itens.Select(i => new EncomendaItemDTO
                {
                    NomeProduto = i.Produto.Nome,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            }).ToList();

            return Ok(listaDto);
        }

        [HttpGet("fornecedor/{fornecedorId}/vendas")]
        public async Task<ActionResult<List<VendaFornecedorDTO>>> GetVendasFornecedor(string fornecedorId)
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
