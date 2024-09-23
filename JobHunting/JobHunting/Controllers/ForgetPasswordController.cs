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
        private readonly CandidateForgetPasswordEmailService _CandidateForgetPasswordEmailService;
        private readonly CompanyForgetPasswordEmailService _CompanyForgetPasswordEmailService;
        public ForgetPasswordController(DuckContext context, CandidateForgetPasswordEmailService candidateForgetPasswordEmailService, CompanyForgetPasswordEmailService companyForgetPasswordEmailService)
        {
            _context = context;
            _CandidateForgetPasswordEmailService = candidateForgetPasswordEmailService;
            _CompanyForgetPasswordEmailService = companyForgetPasswordEmailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ForgetPasswordMail()
        {
            return View();
        }

        public IActionResult CandidateResetPassword()
        {

            string email = TempData["candidateEmail"].ToString();

            var td = new CandidateResetPasswordInputModel //把tempdata的email傳到viewmodel
            {
                Email = email
            };
            return View(td);
        }
        public IActionResult CompanyResetPassword()
        {
            string ContactEmail = TempData["companyEmail"].ToString();

            var td = new CompanyResetPasswordInputModel //把tempdata的email傳到viewmodel
            {
                ContactEmail = ContactEmail
            };


            return View(td);
        }



        [HttpGet]
        public IActionResult CandidateVerifyEmail(string token, string email, long expiry)
        {
            string Token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email) || expiry < DateTime.UtcNow.Ticks)
            {
                return BadRequest("驗證連結無效或已過期。");
            }

            //解email的編碼
            string EncodingEmail = Encoding.UTF8.GetString(Convert.FromBase64String(email));

            // 找到對應的求職者mail
            var candidate = _context.Candidates.FirstOrDefault(c => c.Email == EncodingEmail);
            if (candidate == null)
            {
                return BadRequest("無效的驗證連結。");
            }

            TempData["candidateEmail"] = EncodingEmail;

            // 跳轉到修改密碼頁面
            return RedirectToAction("CandidateResetPassword", "ForgetPassword");

        }

        [HttpGet]
        public IActionResult CompanyVerifyEmail(string token, string email, long expiry)
        {
            string Token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email) || expiry < DateTime.UtcNow.Ticks)
            {
                return BadRequest("驗證連結無效或已過期。");
            }

            //解email的編碼
            string EncodingEmail = Encoding.UTF8.GetString(Convert.FromBase64String(email));

            // 找到對應的求職者mail
            var companies = _context.Companies.FirstOrDefault(c => c.ContactEmail == EncodingEmail);
            if (companies == null)
            {
                return BadRequest("無效的驗證連結。");
            }

            TempData["companyEmail"] = EncodingEmail;
            // 跳轉到修改密碼頁面
            return RedirectToAction("CompanyResetPassword", "ForgetPassword");

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

                _CandidateForgetPasswordEmailService.SendEmail(fpc.Email, "重置密碼信件");
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
                _CompanyForgetPasswordEmailService.SendEmail(fpc.ContactEmail, "重置密碼信件");
                return Json(new { success = true, message = "已發送重置信件至您的信箱" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "發送信件失敗" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> CandidateResetPassword([FromBody] CandidateResetPasswordInputModel crp)
        {
            try
            {
                if(crp.Password != crp.ConfirmPassword)
                {
                    return Json(new { success = false, message = "密碼與確認密碼不一致。" });
                }

                if (string.IsNullOrEmpty(crp.Email))
                {
                    return Json(new { success = false, message = "請求無效。" });
                }

                var candidate = _context.Candidates.FirstOrDefault(c => c.Email == crp.Email);
                if (candidate == null)
                {
                    return Json(new { success = false, message = "重置密碼失敗。" });
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(crp.Password);
                candidate.Password = hashedPassword;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "重置密碼成功。" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "重置密碼失敗。" });
            }


        }
        [HttpPost]
        public async Task<IActionResult> CompanyResetPassword([FromBody] CompanyResetPasswordInputModel crp)
        {
            try
            {
                if (crp.Password != crp.ConfirmPassword)
                {
                    return Json(new { success = false, message = "密碼與確認密碼不一致。" });
                }

                if (string.IsNullOrEmpty(crp.ContactEmail))
                {
                    return Json(new { success = false, message = "請求無效。" });
                }

                var companies = _context.Companies.FirstOrDefault(c => c.ContactEmail == crp.ContactEmail);
                if (companies == null)
                {
                    return Json(new { success = false, message = "重置密碼失敗。" });
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(crp.Password);
                companies.Password = hashedPassword;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "重置密碼成功。" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "重置密碼失敗。" });
            }


        }


    }
}
