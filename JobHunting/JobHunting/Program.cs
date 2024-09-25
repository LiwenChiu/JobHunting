using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Data;
using JobHunting.Models;
using JobHunting.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hangfire;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
//發送註冊信件
builder.Services.AddSingleton<EmailService>();
//重新發送驗證信
builder.Services.AddSingleton<ReviewMaillService>();
//求職者忘記密碼
builder.Services.AddSingleton<CandidateForgetPasswordEmailService>();
//公司忘記密碼
builder.Services.AddSingleton<CompanyForgetPasswordEmailService>();
//公司訂閱天數即將到期Email通知
builder.Services.AddSingleton<PlanExpiredEmailService>();
//定時到藍新金流檢查Status==false的訂單狀態，再改寫或取消DB的Data
//builder.Services.AddSingleton<NewebPaySearchService>();

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

// Add Hangfire services
builder.Services.AddHangfire(configuration =>
    configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("Duck")));

// Add Hangfire server
builder.Services.AddHangfireServer(); // 過去在 UseHangfireServer 的配置現在移到這裡
builder.Services.AddScoped<PlanExpiredService>();
//builder.Services.AddScoped<NewebPaySearchService>();
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

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
    name: "Companies",
    areaName: "Companies",
    pattern: "Companies/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "Admins",
    areaName: "Admins",
    pattern: "Admins/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "Candidates",
    areaName: "Candidates",
    pattern: "Candidates/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.UseHangfireDashboard();

using (var scope = app.Services.CreateScope())
{
    var companyService = scope.ServiceProvider.GetRequiredService<PlanExpiredService>();
    //var NewebPaySearchService = scope.ServiceProvider.GetRequiredService<NewebPaySearchService>();

    var options = new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time")
    };
    // 每天執行一次，檢查即將到期的公司並發送提醒
    RecurringJob.AddOrUpdate(
        "SendExpirationReminders",
    () => companyService.SendExpirationReminders(),
    "59 23 * * *",
    options);

    // 每天執行一次，檢查過期的公司並關閉其職缺
    RecurringJob.AddOrUpdate(
        "CloseExpiredJobOpenings",
    () => companyService.CloseExpiredJobOpenings(),
    "59 23 * * *",
    options);

    //RecurringJob.AddOrUpdate("NewebPaySearchStatusFalse", () => NewebPaySearchService.NewebPaySearchStatusFalse(), "0 0 * ? ***", options);
}

app.Run();
