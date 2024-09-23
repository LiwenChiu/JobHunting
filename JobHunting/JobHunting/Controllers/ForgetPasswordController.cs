using JobHunting.Models;
using JobHunting.Services;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

namespace JobHunting.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private readonly DuckContext _context;
        private readonly ForgetPasswordEmailService _ForgetPasswordEmailService;
        public ForgetPasswordController(DuckContext context, ForgetPasswordEmailService forgetPasswordEmailService)
        {
            _context = context;
            _ForgetPasswordEmailService = forgetPasswordEmailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ForgetPasswordMail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerifyEmail(string token, string email, long expiry)
        {
            string Token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email) || expiry < DateTime.UtcNow.Ticks)
            {
                return BadRequest("驗證連結無效或已過期。");
            }

            //解email的編碼
            string Email = Encoding.UTF8.GetString(Convert.FromBase64String(email));

            // 找到對應的求職者mail
            var candidate = _context.Candidates.FirstOrDefault(c => c.Email == Email);
            if (candidate == null)
            {
                return BadRequest("無效的驗證連結。");
            }


            // 跳轉到修改密碼頁面
            return RedirectToAction("Login", "Home");

        }


        [HttpPost]
        public async Task<IActionResult> ForgetPasswordCandidate([FromBody] ForgetPasswordCandidateViewModel fpc)
        {
            try
            {
                var candidateE =  _context.Candidates.FirstOrDefault(c => c.Email == fpc.Email);
                if (candidateE == null)
                {
                    return Json(new { success = false, message = "此電子信箱未註冊過" });
                }

                var candidateN =  _context.Candidates.FirstOrDefault(c => c.NationalId == fpc.NationalId);
                if (candidateN == null)
                {
                    return Json(new { success = false, message = "此身份證未註冊過" });
                }

                _ForgetPasswordEmailService.SendEmail(fpc.Email, "重置密碼信件");
                return Json(new { success = true, message = "已發送重置信件至您的信箱" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "發送信件失敗" });
            }



        }

        [HttpPost]
        public async Task<IActionResult> ForgetPasswordcompany([FromBody] ForgetPasswordCompanyViewModel fpc)
        {
            try
            {
                var companiesE = _context.Companies.FirstOrDefault(c => c.ContactEmail == fpc.ContactEmail);
                if (companiesE == null)
                {
                    return Json(new { success = false, message = "此電子信箱未註冊過" });
                }

                var companiesG = _context.Companies.FirstOrDefault(c => c.GUINumber == fpc.GUINumber);
                if (companiesG == null)
                {
                    return Json(new { success = false, message = "此統一編號未註冊過" });
                }


                //寄信
                _ForgetPasswordEmailService.SendEmail(fpc.ContactEmail, "重置密碼信件");
                return Json(new { success = true, message = "已發送重置信件至您的信箱" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "發送信件失敗" });
            }



        }
    }
}
