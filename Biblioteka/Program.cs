using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Biblioteka.Dane;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja po³¹czenia z baz¹ danych
var connectionString = builder.Configuration.GetConnectionString("BibliotekaDB");
builder.Services.AddDbContext<BibliotekaContext>(options => options.UseSqlServer(connectionString));

// Dodanie us³ug MVC
builder.Services.AddControllersWithViews();

// Dodanie sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas wygaœniêcia sesji
    options.Cookie.HttpOnly = true; // Zapobieganie dostêpowi do ciasteczek sesji z JavaScript
    options.Cookie.IsEssential = true; // Wymagane dla GDPR
});

// Dodanie polityki autoryzacji
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLogin", policy =>
    {
        policy.RequireAuthenticatedUser(); // Wymagane zalogowanie u¿ytkownika
    });
});

// Dodanie mechanizmu uwierzytelniania
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Konto/Logowanie";
        options.LogoutPath = "/Konto/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

var app = builder.Build();

// Konfiguracja œrodowiska
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// U¿ycie HTTPS
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// U¿ycie sesji
app.UseSession();

// U¿ycie polityki autoryzacji
app.UseAuthorization();

// U¿ycie mechanizmu uwierzytelniania
app.UseAuthentication();

// Konfiguracja domyœlnego routingu
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=StronaGlowna}/{action=Index}/{id?}");

app.Run();
