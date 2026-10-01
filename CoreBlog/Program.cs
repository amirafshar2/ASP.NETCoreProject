using BE.Concrete;
using BLL.Container;
using CoreBlog.Infrastructure;
using DAL.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ---------- Datenbank (SQLite – liegt im Projektordner App_Data) ----------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=App_Data/coreblog.db";
var dbPath = connectionString.Replace("Data Source=", "").Trim();
if (!Path.IsPathRooted(dbPath))
{
    dbPath = Path.Combine(builder.Environment.ContentRootPath, dbPath);
    connectionString = $"Data Source={dbPath}";
}
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
builder.Services.AddDbContext<Context>(o => o.UseSqlite(connectionString));

// ---------- Identity mit Rollen (Admin, Writer) ----------
builder.Services.AddIdentity<AppUser, AppRole>(o =>
    {
        o.Password.RequiredLength = 8;
        o.Password.RequireNonAlphanumeric = false;
        o.User.RequireUniqueEmail = true;
        o.Lockout.MaxFailedAccessAttempts = 10;
    })
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<GermanIdentityErrorDescriber>();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/Account/Login";
    o.LogoutPath = "/Account/Logout";
    o.AccessDeniedPath = "/Account/AccessDenied";
    o.Cookie.Name = "CoreBlog.Auth";
    o.ExpireTimeSpan = TimeSpan.FromHours(8);
    o.SlidingExpiration = true;
});

// ---------- Schichten & Dienste ----------
builder.Services.AddBusinessServices();
builder.Services.Configure<DemoOptions>(builder.Configuration.GetSection("Demo"));
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<ImageUploader>();
builder.Services.AddHostedService<DemoResetService>();

builder.Services.AddControllersWithViews(o =>
    o.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddRouting(o => o.LowercaseUrls = true);

var app = builder.Build();

// ---------- Datenbank anlegen / Demo zurücksetzen ----------
var demo = builder.Configuration.GetSection("Demo").Get<DemoOptions>() ?? new DemoOptions();
await DatabaseInitializer.InitializeAsync(app.Services, reset: demo.Enabled && demo.ResetOnStartup);

// ---------- Pipeline ----------
var culture = new CultureInfo("de-DE");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = new[] { culture },
    SupportedUICultures = new[] { culture }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("areas", "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute("blog", "blog/{id:int}/{slug?}", new { controller = "Blog", action = "Detail" });
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
