using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("historico")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<List<Encomenda>>> Historico()
        {
            var clienteIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (clienteIdClaim == null) return Unauthorized();

            int clienteId = int.Parse(clienteIdClaim.Value);

            var historico = await _context.Encomendas
                .Include(e => e.Itens)
                .ThenInclude(i => i.Produto)
                .Where(e => e.ClienteId == clienteId)
                .OrderByDescending(e => e.Data_Encomenda)
                .ToListAsync();

            return Ok(historico);
        }
    }
}
