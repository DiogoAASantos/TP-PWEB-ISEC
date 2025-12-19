using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace RCL.Data.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        // Injeta o LocalStorage para obter o token
        public AuthTokenHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = null;

            try
            {
                token = await _localStorage.GetItemAsStringAsync("authToken");
            }
            catch (InvalidOperationException) {}
            catch (Exception) {}

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine("DEBUG: Token enviado no header!");
            }
            else
            {
                Console.WriteLine("DEBUG: Nenhum token encontrado no LocalStorage!");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
