using API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;
using RCL.Data.DTO.Auth;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ApplicationDbContext _context;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Tipo = dto.TipoUtilizador
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.TipoUtilizador.ToString());

            switch (dto.TipoUtilizador)
            {
                case TipoUtilizador.Cliente:
                    _context.Clientes.Add(new Cliente
                    {
                        Nome = dto.Nome,
                        Email = dto.Email,
                        Estado = EstadoUtilizador.Pendente,
                    });
                    break;

                case TipoUtilizador.Fornecedor:
                    _context.Fornecedores.Add(new Fornecedor
                    {
                        Nome = dto.Nome,
                        Email = dto.Email,
                        Estado = EstadoUtilizador.Pendente
                    });
                    break;
            }

            await _context.SaveChangesAsync();

            return Ok("Utilizador criado com sucesso");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
            if (!result.Succeeded) return Unauthorized("Credenciais inválidas");

            // Verifica se é cliente ou fornecedor
            var cliente = await _userManager.Users.OfType<Cliente>().FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (cliente != null)
            {
                return Ok(new UserDTO
                {
                    Id = cliente.Id,
                    Email = cliente.Email,
                    Nome = cliente.Nome,
                    Tipo = "Cliente"
                });
            }

            var fornecedor = await _userManager.Users.OfType<Fornecedor>().FirstOrDefaultAsync(f => f.Email == dto.Email);
            if (fornecedor != null)
            {
                return Ok(new UserDTO
                {
                    Id = fornecedor.Id,
                    Email = fornecedor.Email,
                    Nome = fornecedor.Nome,
                    Tipo = "Fornecedor"
                });
            }

            return Unauthorized("Utilizador não encontrado");
        }

        public record RegisterDto(string Nome, string Email, string Password, TipoUtilizador Tipo);
        public record LoginDto(string Email, string Password);

    }
}
