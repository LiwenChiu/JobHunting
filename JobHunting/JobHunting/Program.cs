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
    options.Password.RequireDigit = true; //���ԧB�Ʀr
    options.Password.RequireLowercase = true; //�p�g�^��r
    options.Password.RequireNonAlphanumeric = true; //�S��Ÿ�
    options.Password.RequireUppercase = true; //�j�g�^��r
    options.Password.RequiredLength = 8; //�n�D8�Ӧr
    options.Password.RequiredUniqueChars = 1; //�ܤ֭n���@�Ӥ����ƪ��r��

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //�b����w������
    options.Lockout.MaxFailedAccessAttempts = 3; //��3���n��b��
    options.Lockout.AllowedForNewUsers = true; //�ئn�b������A�n�q�l�l������

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //�b�����\��r
    options.User.RequireUniqueEmail = true; //�q�l�l�󤣯୫��
    options.SignIn.RequireConfirmedEmail = true; //�q�l�l��T�{�A�n���H����
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
