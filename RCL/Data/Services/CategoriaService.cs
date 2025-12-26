using RCL.Data.DTO;
using RCL.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _http;

        public CategoriaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CategoriaDTO>> ObterCategoriasAsync()
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<CategoriaDTO>>("api/categoria");
                return resultado ?? new List<CategoriaDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar categorias: {ex.Message}");
                return new List<CategoriaDTO>();
            }
        }
    }
}
