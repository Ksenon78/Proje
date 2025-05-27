using App.Data; // DbContext'in namespace'i
using App.Data.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. DbContext'i ve connection string'i ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthentication().AddCookie(Settings.AuthCookieName, 
    options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Forbidden";
        options.Cookie.Name = Settings.AuthCookieName;

        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Çerezler yalnýzca HTTPS üzerinden gönderilsin
        options.Cookie.SameSite = SameSiteMode.None; // Üst domainler arasý kullanýlabilir olsun
        options.Cookie.IsEssential = true; // Kimlik doðrulama için gerekli olduðunu belirle
    });

// 2. Identity servisini ekle
  builder.Services.AddAuthorization(options  =>

  {
      options.AddPolicy("Admin", policy => policy.RequireClaim("admin", "true"));
  });


builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(); // burasý routing'den sonra ama authentication'dan önce
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
