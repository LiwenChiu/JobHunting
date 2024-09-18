using Azure.Core;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JobHunting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DuckContext _context;
        public HomeController(ILogger<HomeController> logger, DuckContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> AddLetter([FromForm] InsterLetter letter)
        {

            OpinionLetter opinionLetter = new OpinionLetter();
            opinionLetter.CompanyId = letter.CompanyId;
            opinionLetter.Class = letter.Letterclass;
            opinionLetter.SubjectLine = letter.SubjectLine;
            opinionLetter.Content = letter.Content;
            opinionLetter.SendTime = letter.SendTime;
            IsPicture(letter, opinionLetter);
            _context.OpinionLetters.Add(opinionLetter);
            await _context.SaveChangesAsync();
            
            return "新增信件成功";
        }
        private static void IsPicture(InsterLetter letter, OpinionLetter o)
        {
            if (letter.ImageFile != null)
            {
                using (BinaryReader br = new BinaryReader(letter.ImageFile.OpenReadStream()))
                {
                    o.Attachment = br.ReadBytes((int)letter.ImageFile.Length);
                }
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoLogin([FromBody] CompositeLogin loginRequest)
        {
            if (loginRequest.Role == "candidate")
            {
                var candidateLogin = loginRequest.CandidateLoginVM;

                // 求職者驗證邏輯
                if (ValidateCandidate(candidateLogin))
                {
                    // 驗證通過，建立 claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, candidateLogin.NationalId),  // 使用身分證字號作為名稱
                new Claim(ClaimTypes.Role, "candidate")                 // 設定角色為 candidate
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 使用 Cookie 認證進行登入
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, message = "求職者登入成功", role = "candidate" });
                }
                else
                {
                    return Json(new { success = false, message = "求職者登入失敗：帳號或密碼錯誤" });
                }
            }
            else if (loginRequest.Role == "company")
            {
                var companyLogin = loginRequest.CompanyLoginVM;

                // 公司驗證邏輯
                if (ValidateCompany(companyLogin))
                {
                    // 驗證通過，建立 claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, companyLogin.GUINumber),    // 使用統一編號作為名稱
                new Claim(ClaimTypes.Role, "company")                  // 設定角色為 company
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 使用 Cookie 認證進行登入
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, message = "公司登入成功", role = "company" });
                }
                else
                {
                    return Json(new { success = false, message = "公司登入失敗：統一編號或密碼錯誤" });
                }
            }

            return Json(new { success = false, message = "無效的角色" });
        }


        private bool ValidateCandidate(CandidateLoginVM candidateLogin)
        {
            var candidate = _context.Candidates
            .FirstOrDefault(c => c.NationalId == candidateLogin.NationalId && c.Email == candidateLogin.Email);

            if (candidate == null)
            {
                return false;
            }

            // 比對密碼，假設密碼是明文儲存的，實際情況應該使用加密
            return candidate.Password == candidateLogin.Password;
        }

        private bool ValidateCompany(CompanyLoginVM companyLogin)
        {
            var company = _context.Companies
            .FirstOrDefault(c => c.GUINumber == companyLogin.GUINumber);

            if (company == null)
            {
                return false;
            }

            // 比對密碼，假設密碼是明文儲存的，實際情況應該使用加密
            return company.Password == companyLogin.Password;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 執行登出操作，清除使用者登入資訊
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 重導向到登入頁面或首頁
            return RedirectToAction("Login", "Home"); // 或 RedirectToAction("Index", "Home");
        }
    }
}
