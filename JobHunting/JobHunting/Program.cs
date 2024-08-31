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

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = true; //阿拉伯數字
    options.Password.RequireLowercase = true; //小寫英文字
    options.Password.RequireNonAlphanumeric = true; //特殊符號
    options.Password.RequireUppercase = true; //大寫英文字
    options.Password.RequiredLength = 8; //要求8個字
    options.Password.RequiredUniqueChars = 1; //至少要有一個不重複的字元

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //帳號鎖定五分鐘
    options.Lockout.MaxFailedAccessAttempts = 3; //錯3次要鎖帳號
    options.Lockout.AllowedForNewUsers = true; //建好帳號先鎖，要電子郵件驗證

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //帳號允許文字
    options.User.RequireUniqueEmail = true; //電子郵件不能重複
    options.SignIn.RequireConfirmedEmail = true; //電子郵件確認，要收信驗證
});
builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
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
