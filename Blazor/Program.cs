using Microsoft.AspNetCore.Components.Authorization;
using RCL.Data.Interfaces;
using RCL.Data.Services;
using BlazorWEB.Components;
using Blazored.LocalStorage;
using BlazorWEB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options => options.DetailedErrors = true);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IMyStorageService, WebStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
});


builder.Services.AddHttpClient<IEncomendaService, EncomendaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
});

builder.Services.AddHttpClient<ICarrinhoService, CarrinhoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
});

builder.Services.AddHttpClient<IClienteService, ClienteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
});

builder.Services.AddHttpClient<IFornecedorService, FornecedorService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
});

builder.Services.AddHttpClient<IProdutoService, ProdutoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
});

builder.Services.AddHttpClient<ICategoriaService, CategoriaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
});

builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7000");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(RCL.Shared.Layout.MainLayout).Assembly);

app.Run();