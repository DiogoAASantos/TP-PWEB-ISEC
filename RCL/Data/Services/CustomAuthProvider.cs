using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using RCL.Data.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace RCL.Data.Services
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly IMyStorageService _localStorage; 

        public CustomAuthProvider(IMyStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = null;

            try
            {
                token = await _localStorage.GetItemAsync<string>("authToken");
            }
            catch (Exception) { }

            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    token = token.Trim().Trim('"');

                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                    if (jwtToken.ValidTo > DateTime.UtcNow)
                    {
                        identity = new ClaimsIdentity(jwtToken.Claims, "jwt");

                        var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == ClaimTypes.NameIdentifier);
                        if (idClaim != null)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, idClaim.Value));
                        }

                        var roleClaims = jwtToken.Claims.Where(c => c.Type == "role" || c.Type == ClaimTypes.Role);
                        foreach (var r in roleClaims)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, r.Value));
                        }
                    }
                    else
                    {
                        await _localStorage.RemoveItemAsync("authToken");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro no JWT: {ex.Message}");
                    await _localStorage.RemoveItemAsync("authToken");
                }
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyUserLoggedIn()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLoggedOut()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}