using API.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCL.Data.Model;
using RCL.Data.DTO.Auth;
using RCL.Data.Model.Enums;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ApplicationDbContext context,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            // 1. Instanciar o tipo correto logo no início
            ApplicationUser user;

            if (dto.TipoUtilizador == TipoUtilizador.Cliente)
            {
                user = new Cliente
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    Nome = dto.Nome,
                    Tipo = dto.TipoUtilizador,
                    Estado = EstadoUtilizador.Pendente
                };
            }
            else
            {
                user = new Fornecedor
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    Nome = dto.Nome,
                    Tipo = dto.TipoUtilizador,
                    Estado = EstadoUtilizador.Pendente
                };
            }

            // 2. O CreateAsync já guarda tudo na base de dados (Users e a tabela de Cliente/Fornecedor)
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.TipoUtilizador.ToString());

            // NÃO precisas de _context.Clientes.Add nem de _context.SaveChangesAsync() 
            // porque o CreateAsync já fez o trabalho se a herança estiver bem configurada.

            return Ok("Utilizador criado com sucesso");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // 1. A TUA LÓGICA ORIGINAL (Validação de Password)
            // Nota: O último 'false' impede o bloqueio da conta em falhas para facilitar testes
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized("Credenciais inválidas");

            UserDTO? userResponse = null;
            string role = "";

            // 2. A TUA LÓGICA ORIGINAL (Identificar se é Cliente ou Fornecedor)
            // Usamos OfType para garantir que vamos buscar os dados específicos

            var cliente = await _userManager.Users.OfType<Cliente>().FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (cliente != null)
            {
                role = "Cliente";
                Console.WriteLine($"DEBUG: Cliente encontrado. Nome na BD: '{cliente.Nome}'");
                userResponse = new UserDTO
                {
                    Id = cliente.Id,
                    Email = cliente.Email!,
                    Nome = cliente.Nome!, // Propriedade específica de Cliente
                    Tipo = role
                };
            }
            else
            {
                // Se não for cliente, verifica se é fornecedor
                var fornecedor = await _userManager.Users.OfType<Fornecedor>().FirstOrDefaultAsync(f => f.Email == dto.Email);
                if (fornecedor != null)
                {
                    role = "Fornecedor";
                    userResponse = new UserDTO
                    {
                        Id = fornecedor.Id,
                        Email = fornecedor.Email!,
                        Nome = fornecedor.Nome!, // Propriedade específica de Fornecedor
                        Tipo = role
                    };
                }
            }

            if (userResponse == null)
                return Unauthorized("Utilizador existe mas não tem perfil de Cliente nem Fornecedor.");

            // 3. A NOVIDADE: GERAR O TOKEN JWT
            // Sem isto, o login é inútil para o MAUI/Blazor
            userResponse.Token = GerarTokenJwt(userResponse.Id, userResponse.Email, role);

            return Ok(userResponse);
        }

        private string GerarTokenJwt(string userId, string email, string role)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                // Usa ClaimTypes para garantir compatibilidade total
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token dura 7 dias
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public record RegisterDto(string Nome, string Email, string Password, TipoUtilizador Tipo);
        public record LoginDto(string Email, string Password);

    }
}
