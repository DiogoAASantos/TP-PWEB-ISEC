using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public class ClienteService : IClienteService
    {
        private readonly Cliente _cliente; // cliente atual
        private readonly AppDbContext _context; // contexto da base de dados (opcional)

        public ClienteService(Cliente cliente, AppDbContext context)
        {
            _cliente = cliente;
            _context = context;
        }

        public async Task<Cliente?> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            // Consulta na base de dados
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == email && c.Password == password && c.Estado == "Activo");

            return cliente;
        }

        public async Task<Encomenda> EfetivarCompraAsync()
        {
            if (!_cliente.Carrinho.Any())
                throw new InvalidOperationException("Carrinho vazio.");

            var itensAgrupados = _cliente.Carrinho
                .GroupBy(p => p.Id)
                .Select(g => new EncomendaItem
                {
                    ProdutoId = g.Key,
                    Quantidade = g.Count(),
                    PrecoUnitario = g.First().Preco
                })
                .ToList();

            var novaEncomenda = new Encomenda
            {
                Id_Cliente = _cliente.Id,
                Data_Encomenda = DateTime.Now,
                itens = itensAgrupados,
                Total = itensAgrupados.Sum(i => i.Quantidade * i.PrecoUnitario)
            };

            // Limpar carrinho após efetivar compra
            _cliente.Carrinho.Clear();

            // Persistir no banco (se houver)
            if (_context != null)
            {
                _context.Encomendas.Add(novaEncomenda);
                await _context.SaveChangesAsync();
            }

            return novaEncomenda;
        }

        public async Task<List<Encomenda>> ConsultarHistoricoComprasAsync(int clienteId)
        {
            // Consulta todas as encomendas do cliente na base de dados
            var historico = await _context.Encomendas
                .Where(e => e.ClienteId == clienteId)
                .Include(e => e.Itens)          // Inclui os itens da encomenda
                .ThenInclude(i => i.Produto)    // Inclui os detalhes de cada produto
                .OrderByDescending(e => e.DataEncomenda) // Mais recentes primeiro
                .ToListAsync();

            return historico;
        }
    }
}
