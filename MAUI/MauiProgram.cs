using Microsoft.Extensions.Logging;
using RCL.Data.Interfaces;
using RCL.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace MyColl.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // 1. ENDEREÇO BASE (TRUQUE DO ANDROID)
            string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                ? "https://10.0.2.2:7000"
                : "https://localhost:7000";

            // REGISTAR O LOCAL STORAGE
            builder.Services.AddBlazoredLocalStorage();

            // 2. REGISTAR O HANDLER E O HTTPCLIENT
            builder.Services.AddScoped<AuthTokenHandler>();

            builder.Services.AddScoped(sp =>
            {
                // Handler para ignorar SSL
                HttpClientHandler sslHandler = new HttpClientHandler();
                sslHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                // Obtém o Token Handler do DI
                var tokenHandler = sp.GetRequiredService<AuthTokenHandler>();

                // Encaixa o SSL Handler DENTRO do Token Handler
                tokenHandler.InnerHandler = sslHandler;

                // O HttpClient usa o Token Handler como o seu handler principal
                return new HttpClient(tokenHandler) { BaseAddress = new Uri(baseAddress) };
            });


            // 2. REGISTAR OS MESMOS SERVIÇOS QUE NO WEB
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
            builder.Services.AddScoped<IEncomendaService, EncomendaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IFornecedorService, FornecedorService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddAuthorizationCore(); 
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

            return builder.Build();
        }
    }
}