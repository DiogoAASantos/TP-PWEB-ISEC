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

            string baseAddress = "https://72qc49t4-7000.uks1.devtunnels.ms/";

            builder.Services.AddSingleton<IMyStorageService, MauiStorageService>();
            builder.Services.AddBlazoredLocalStorage();


            builder.Services.AddScoped(sp =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                return new HttpClient(handler) { BaseAddress = new Uri(baseAddress) };
            });


            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
            builder.Services.AddScoped<IEncomendaService, EncomendaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IFornecedorService, FornecedorService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();

            builder.Services.AddAuthorizationCore(); 
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

            return builder.Build();
        }
    }
}