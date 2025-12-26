using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.DTO;
using RCL.Data.Model;
using RCL.Data.Model.Enums;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<CategoriaDTO>>> GetCategorias()
        {
            try
            {
                var categorias = await _context.Categoria
                    .Select(c => new CategoriaDTO
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    })
                    .ToListAsync();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERRO NO SERVIDOR: {ex.Message} | {ex.InnerException?.Message}");
            }
        }

    }
}
