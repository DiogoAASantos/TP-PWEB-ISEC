using RCL.Data.Model;
using RCL.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using RCL.Data.Interfaces;

namespace RCL.Data.Services
{
        public class AuthService : IAuthService
        {
            private readonly HttpClient _http;

            public AuthService(HttpClient http) => _http = http;

            public async Task<bool> LoginAsync(LoginDTO dto)
            {
                var response = await _http.PostAsJsonAsync("/api/auth/login", dto);

                if (!response.IsSuccessStatusCode)
                    return false;
                
                return true;
            }

            public async Task<bool> RegisterAsync(RegisterDTO dto)
            {
            var res = await _http.PostAsJsonAsync("auth/register", dto);
            return res.IsSuccessStatusCode;
        }
    }
}
