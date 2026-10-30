using ProjetoTcc.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona suporte para Razor Pages
builder.Services.AddRazorPages();

// 2. Registro do banco de dados SQLite
builder.Services.AddScoped<DepoimentosDB>();

// 3. Habilita suporte a sessão para o login e autenticação do Admin
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Habilita o uso das sessões no pipeline HTTP
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();