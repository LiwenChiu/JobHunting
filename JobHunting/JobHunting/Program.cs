using Admins.Areas.Admins.Models;
using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Data;
using JobHunting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

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

app.UseRouting();

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
