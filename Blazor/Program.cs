using Microsoft.AspNetCore.Components.Authorization;
using RCL.Data.Interfaces;
using RCL.Data.Services;
using BlazorWEB.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 1️⃣ HttpClient para chamar a API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7268/"); // <-- muda para o URL da tua API
});

// 2️⃣ Registar os serviços do RCL (AuthService, ClienteService, etc)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IFornecedorService, FornecedorService>();

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
    .AddInteractiveServerRenderMode();

app.Run();
