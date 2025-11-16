using RCL.Data.Model;

namespace MyColl.RCL.Data.Interfaces
{
    public interface IAdministradorService : IFuncionarioService
    {
        public interface IAdministradorService : IFuncionarioService
        {
            // ===========================
            // Gestão de Funcionários
            // ===========================

            Task<List<Funcionario>> ListarFuncionariosAsync(string? nome = null, bool? ativo = null);
            Task<Funcionario> AdicionarFuncionarioAsync(Funcionario funcionario);
            Task<bool> RemoverFuncionarioAsync(int funcionarioId);
            Task<bool> AtribuirPerfilFuncionarioAsync(int utilizadorId);
            Task<bool> RetirarPerfilFuncionarioAsync(int utilizadorId);
            Task<bool> AlterarEstadoFuncionarioAsync(int funcionarioId, bool ativo);
        }
    }
}