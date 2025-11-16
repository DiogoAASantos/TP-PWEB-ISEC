using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using API.Data;
using RCL.Data.Model;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Tipo = dto.Tipo
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Tipo.ToString());
            return Ok("Utilizador criado com sucesso");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto) 
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
            if (!result.Succeeded) return Unauthorized("Credenciais inválidas");

            // Aqui poderás gerar um JWT no futuro
            return Ok("Login bem-sucedido");
        }
    }

    public record RegisterDto(string Nome, string Email, string Password, TipoUtilizador Tipo);
    public record LoginDto(string Email, string Password);

}
