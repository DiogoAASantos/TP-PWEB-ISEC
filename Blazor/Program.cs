using Microsoft.AspNetCore.Components.Authorization;
using RCL.Data.Interfaces;
using RCL.Data.Services;
using BlazorWEB.Components;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options => options.DetailedErrors = true);

builder.Services.AddAuthorizationCore(); // Habilita o sistema de autorização
builder.Services.AddCascadingAuthenticationState(); // Permite usar <AuthorizeView>

// Regista o nosso fornecedor de autenticação personalizado
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

// REGISTAR O LOCAL STORAGE AQUI
builder.Services.AddBlazoredLocalStorage();




// TOKEN HANDLER
builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddHttpClient("APIClient", client =>
{
    // Usa o endereço da API
    client.BaseAddress = new Uri("https://localhost:7000");
})
    .AddHttpMessageHandler<AuthTokenHandler>(); 

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("APIClient"));


// Registar os serviços do RCL (AuthService, ClienteService, etc)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFornecedorService, FornecedorService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

builder.Services.AddHttpClient<IEncomendaService, EncomendaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<ICarrinhoService, CarrinhoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<IClienteService, ClienteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<IFornecedorService, FornecedorService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<IProdutoService, ProdutoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/");
})
.AddHttpMessageHandler<AuthTokenHandler>();


var app = builder.Build();

// Configure
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
