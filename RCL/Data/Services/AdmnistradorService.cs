using MyColl.RCL.Data.Interfaces;
using RCL.Data.Model;
using System;

namespace MyColl.RCL.Data.Services
{
    public class AdministradorService : FuncionarioService, IAdministradorService
    {
        private readonly AppDbContext _context;

        public AdministradorService(AppDbContext context)
        {
            _context = context;
        }

        // ===========================
        // Gestão de Funcionários
        // ===========================

        public async Task<List<Funcionario>> ListarFuncionariosAsync(string? nome = null, bool? ativo = null)
        {
            var query = _context.Funcionarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(f => f.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

            if (ativo.HasValue)
                query = query.Where(f => f.Ativo == ativo.Value);

            return await Task.FromResult(query.ToList());
        }

        public async Task<Funcionario> AdicionarFuncionarioAsync(Funcionario funcionario)
        {
            if (funcionario == null)
                throw new ArgumentNullException(nameof(funcionario));

            funcionario.Estado = EstadoUtilizador.Ativo;

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return funcionario;
        }

        public async Task<bool> RemoverFuncionarioAsync(int funcionarioId)
        {
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == funcionarioId);
            if (funcionario == null)
                return false;

            // Apenas permite apagar se não tiver histórico de vendas, por exemplo
            bool temDependencias = _context.Vendas.Any(v => v.FuncionarioId == funcionarioId);
            if (temDependencias)
                return false;

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AtribuirPerfilFuncionarioAsync(int utilizadorId)
        {
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.Id == utilizadorId);
            if (utilizador == null)
                return false;

            // Se já é funcionário, não faz nada
            if (_context.Funcionarios.Any(f => f.Id == utilizadorId))
                return false;

            // Converter o utilizador em funcionário
            var funcionario = new Funcionario
            {
                Id = utilizador.Id,
                Nome = utilizador.Nome,
                Email = utilizador.Email,
                PasswordHash = utilizador.PasswordHash,
                Estado = EstadoUtilizador.Ativo,
            };

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RetirarPerfilFuncionarioAsync(int utilizadorId)
        {
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == utilizadorId);
            if (funcionario == null)
                return false;

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlterarEstadoFuncionarioAsync(int funcionarioId, bool ativo)
        {
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == funcionarioId);
            if (funcionario == null)
                return false;

            funcionario.Ativo = ativo;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
