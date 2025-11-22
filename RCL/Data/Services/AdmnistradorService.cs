using MyColl.RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Net.Http.Json;

namespace MyColl.RCL.Data.Services
{
    public class AdministradorService : FuncionarioService, IAdministradorService
    {
        
        public async Task<List<Funcionario>> ListarFuncionariosAsync(string? nome = null, bool? ativo = null)
        {
            
        }

        public async Task<Funcionario> AdicionarFuncionarioAsync(Funcionario funcionario)
        {
            
        }

        public async Task<bool> RemoverFuncionarioAsync(int funcionarioId)
        {
            
        }

        public async Task<bool> AtribuirPerfilFuncionarioAsync(int utilizadorId)
        {
            
        }

        public async Task<bool> RetirarPerfilFuncionarioAsync(int utilizadorId)
        {
           
        }

        public async Task<bool> AlterarEstadoFuncionarioAsync(int funcionarioId, bool ativo)
        {
            
        }
    }
}
