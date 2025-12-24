using Microsoft.AspNetCore.Components.Authorization; 
using RCL.Data.DTO.Auth;
using RCL.Data.Interfaces;
using System.Net.Http.Json;
using Blazored.LocalStorage; 

namespace RCL.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IMyStorageService _localStorage;

        // Injetamos também o AuthenticationStateProvider
        public AuthService(HttpClient http, AuthenticationStateProvider authProvider, IMyStorageService localStorage)
        {
            _http = http;
            _authProvider = authProvider;
            _localStorage = localStorage;
        }

        public async Task<UserDTO?> LoginAsync(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            var user = await response.Content.ReadFromJsonAsync<UserDTO>();

            if (user != null && !string.IsNullOrEmpty(user.Token))
            {
                var cleanToken = user.Token.Trim().Trim('"');

                await _localStorage.SetItemAsync("authToken", cleanToken);

                if (_authProvider is CustomAuthProvider customAuth)
                {
                    customAuth.NotifyUserLoggedIn();
                }
            }

            return user;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var res = await _http.PostAsJsonAsync("api/auth/register", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            // 1. Limpar LocalStorage
            await _localStorage.RemoveItemAsync("authToken");

            // 2. Limpar o cabeçalho do HttpClient (caso tenha sido injetado algum)
            _http.DefaultRequestHeaders.Authorization = null;

            // 3. Notificar o sistema
            if (_authProvider is CustomAuthProvider customAuth)
            {
                customAuth.NotifyUserLoggedOut();
            }
        }
    }
}