using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Data;
using JobHunting.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DuckContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Duck"));
});
builder.Services.AddDbContext<DuckAdminsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Duck"));
});

builder.Services.AddDbContext<DuckCandidatesContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Duck"));
});

builder.Services.AddDbContext<DuckCompaniesContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Duck"));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Home/Login";  // 設定登入頁面
    //options.AccessDeniedPath = "/Account/AccessDenied";  // 設定拒絕存取頁面
}).AddCookie("AdminScheme", options =>
    options.LoginPath = "/Admins/Home/Login"

);

builder.Services.AddAuthorization();  // 添加授權服務
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<IgnoreRouteMiddleware>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "myAreaCompanies",
    areaName: "Companies",
    pattern: "Companies/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "myAreaAdmins",
    areaName: "Admins",
    pattern: "Admins/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "myAreaCandidates",
    areaName: "Candidates",
    pattern: "Candidates/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
 