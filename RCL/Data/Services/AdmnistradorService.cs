using MyColl.RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Net.Http.Json;

namespace MyColl.RCL.Data.Services
{
    public class AdministradorService : FuncionarioService, IAdministradorService
    {
        private readonly HttpClient _http;

        public AdministradorService(HttpClient http)
        {
            _http = http;
        }

        // ===========================
        // Gestão de Funcionários
        // ===========================

        public async Task<List<Funcionario>> ListarFuncionariosAsync(string? nome = null, bool? ativo = null)
        {
            var url = "/api/administradores/funcionarios";
            if (!string.IsNullOrWhiteSpace(nome)) url += $"?nome={nome}";
            if (ativo.HasValue) url += $"{(url.Contains("?") ? "&" : "?")}ativo={ativo.Value}";

            return await _http.GetFromJsonAsync<List<Funcionario>>(url);
        }

        public async Task<Funcionario> AdicionarFuncionarioAsync(Funcionario funcionario)
        {
            var response = await _http.PostAsJsonAsync("/api/administradores/funcionarios", funcionario);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Funcionario>();
        }

        public async Task<bool> RemoverFuncionarioAsync(int funcionarioId)
        {
            var response = await _http.DeleteAsync($"/api/administradores/funcionarios/{funcionarioId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AtribuirPerfilFuncionarioAsync(int utilizadorId)
        {
            var response = await _http.PostAsync($"/api/administradores/funcionarios/{utilizadorId}/atribuir", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RetirarPerfilFuncionarioAsync(int utilizadorId)
        {
            var response = await _http.PostAsync($"/api/administradores/funcionarios/{utilizadorId}/retirar", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AlterarEstadoFuncionarioAsync(int funcionarioId, bool ativo)
        {
            var response = await _http.PutAsJsonAsync($"/api/administradores/funcionarios/{funcionarioId}/estado", new { ativo });
            return response.IsSuccessStatusCode;
        }
    }
}
