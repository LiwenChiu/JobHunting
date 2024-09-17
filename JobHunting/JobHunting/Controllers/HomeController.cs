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
            
            return "�s�W�H�󦨥\";
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

                // �D¾�������޿�
                if (ValidateCandidate(candidateLogin))
                {
                    // ���ҳq�L�A�إ� claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, candidateLogin.NationalId),  // �ϥΨ����Ҧr���@���W��
                new Claim(ClaimTypes.Role, "candidate")                 // �]�w���⬰ candidate
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // �ϥ� Cookie �{�Ҷi��n�J
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, message = "�D¾�̵n�J���\", role = "candidate" });
                }
                else
                {
                    return Json(new { success = false, message = "�D¾�̵n�J���ѡG�b���αK�X���~" });
                }
            }
            else if (loginRequest.Role == "company")
            {
                var companyLogin = loginRequest.CompanyLoginVM;

                // ���q�����޿�
                if (ValidateCompany(companyLogin))
                {
                    // ���ҳq�L�A�إ� claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, companyLogin.GUINumber),    // �ϥβΤ@�s���@���W��
                new Claim(ClaimTypes.Role, "company")                  // �]�w���⬰ company
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // �ϥ� Cookie �{�Ҷi��n�J
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, message = "���q�n�J���\", role = "company" });
                }
                else
                {
                    return Json(new { success = false, message = "���q�n�J���ѡG�Τ@�s���αK�X���~" });
                }
            }

            return Json(new { success = false, message = "�L�Ī�����" });
        }


        private bool ValidateCandidate(CandidateLoginVM candidateLogin)
        {
            var candidate = _context.Candidates
            .FirstOrDefault(c => c.NationalId == candidateLogin.NationalId && c.Email == candidateLogin.Email);

            if (candidate == null)
            {
                return false;
            }

            // ���K�X�A���]�K�X�O�����x�s���A��ڱ��p���ӨϥΥ[�K
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

            // ���K�X�A���]�K�X�O�����x�s���A��ڱ��p���ӨϥΥ[�K
            return company.Password == companyLogin.Password;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // ����n�X�ާ@�A�M���ϥΪ̵n�J��T
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // ���ɦV��n�J�����έ���
            return RedirectToAction("Login", "Home"); // �� RedirectToAction("Index", "Home");
        }
    }
}
