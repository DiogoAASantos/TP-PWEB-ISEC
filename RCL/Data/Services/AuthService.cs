using Microsoft.AspNetCore.Authentication;
using RCL.Data.DTO.Auth;
using RCL.Data.Interfaces;
using RCL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Services
{
        public class AuthService : IAuthService
        {
            private readonly HttpClient _http;

            public AuthService(HttpClient http) => _http = http;

        public async Task<UserDTO?> LoginAsync(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return null; // login falhou

            // Ler o utilizador autenticado vindo do backend
            var user = await response.Content.ReadFromJsonAsync<UserDTO>();
            return user;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
            {
            var res = await _http.PostAsJsonAsync("auth/register", dto);
            return res.IsSuccessStatusCode;
        }
    }
}
