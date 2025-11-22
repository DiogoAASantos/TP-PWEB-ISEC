using API.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoController (ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddCarrinhoAsync(List<(int produtoId, int quantidade)> itens)
        {
            // Aqui o carrinho pode ser mantido em memória ou sessão (dependendo da arquitetura)
            return Task.CompletedTask;
        }
    }
}
