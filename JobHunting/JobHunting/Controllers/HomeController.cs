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
using System.Net;
using Microsoft.Data.SqlClient;

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

        public async Task<IActionResult> OpeningsList(int id)
        {
            return Json(_context.Openings.AsNoTracking().Include(a => a.Company).Include(o => o.Candidates).Select(b => new OpeningsIndexViewModel
            {
                OpeningId = b.OpeningId,
                CompanyId = b.CompanyId,
                Title = b.Title,
                Address = b.Address,
                Description = b.Description,
                Degree = b.Degree,
                Benefits = b.Benefits,
                SalaryMax = b.SalaryMax,
                SalaryMin = b.SalaryMin,
                Time = b.Time,
                ContactEmail = b.ContactEmail,
                ContactName = b.ContactName,
                ContactPhone = b.ContactPhone,
                CompanyName = b.Company.CompanyName,
                LikeYN = b.Candidates.Where(c => c.CandidateId == id).FirstOrDefault() != null,
            }));
        }
        [HttpPost]
        public async Task<string> AddFavorite([FromBody] AddFavoriteOpeningsViewModel favorite)
        {
            try
            {
                var query = "INSERT INTO CandidateOpeningLikeRecords(CandidateId,OpeningId) VALUES (@CandidateId ,@OpeningId)";
                var candidateIdParam = new SqlParameter("@CandidateId", favorite.CandidateId);
                var openingIdParam = new SqlParameter("@OpeningId", favorite.OpeningId);
                await _context.Database.ExecuteSqlRawAsync(query, candidateIdParam, openingIdParam);
            }
            catch (Exception ex)
            {
                return "此職缺已收藏";
            }


            return "職缺已成功收藏";
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> DeleteFavorite([FromBody] DeleteFavoriteOpeningsViewModel dfovm)
        {
            try
            {
                var query = "DELETE FROM CandidateOpeningLikeRecords WHERE CandidateId = @CandidateId AND OpeningId = @OpeningId";
                var candidateIdParam = new SqlParameter("@CandidateId", dfovm.CandidateId);
                var openingIdParam = new SqlParameter("@OpeningId", dfovm.OpeningId);
                await _context.Database.ExecuteSqlRawAsync(query, candidateIdParam, openingIdParam);
            }
            catch(Exception ex)
            {
                return "取消收藏職缺失敗";
            }

            return "取消收藏職缺成功";
        }

        //GET: Home/GetOpening
        public async Task<GetOpeningOutputViewModel> GetOpening(int id)
        {
            var opening = await _context.Openings.AsNoTracking().Include(o => o.Company).Include(o => o.TitleClasses).Include(o => o.Tags)
                .Where(o => o.OpeningId == id)
                .Select(o => new GetOpeningOutputViewModel
                {
                    OpeningTitle = o.Title,
                    CompanyName = o.Company.CompanyName,
                    Address = o.Address,
                    Description = o.Description,
                    Benefit = o.Benefits,
                    Degree = o.Degree,
                    InterviewYN = o.InterviewYN,
                    SalaryMax = o.SalaryMax,
                    SalaryMin = o.SalaryMin,
                    Time = o.Time,
                    ContactName = o.ContactName,
                    ContactEmail = o.ContactEmail,
                    ContactPhone = o.ContactPhone,
                    TitleClassName = o.TitleClasses.Select(tc => tc.TitleClassName),
                    TagName = o.Tags.Select(t => t.TagName),
                })
                .FirstOrDefaultAsync();

            if (opening == null)
            {
                return new GetOpeningOutputViewModel();
            }

            return opening;
        }

        //GET: Home/ResumesJson
        public async Task<IEnumerable<ResumesOutputViewModel>> ResumesJson(int id)
        {
            var resumes = _context.Resumes.AsNoTracking()
                .Where(r => r.CandidateId == id && r.ReleaseYN == true)
                .Select(r => new ResumesOutputViewModel
                {
                    ResumeId = r.ResumeId,
                    ResumeTitle = r.Title,
                });

            return resumes;
        }

        //POST: Home/ApplyJob
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ApplyJobOutputViewModel> ApplyJob([FromBody][Bind("candidateId,resumeId,openingId")] ApplyJobViewModel cajvm)
        {
            if (!ModelState.IsValid)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false,
                };
            }

            var Candidate = await _context.Candidates.FindAsync(cajvm.candidateId);
            if (Candidate == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false,
                };
            }

            var resume = await _context.Resumes.FindAsync(cajvm.resumeId);
            if (resume == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false,
                };
            }

            if (resume.ReleaseYN == false)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "履歷未開放",
                    AlertStatus = false,
                };
            }

            if (Candidate.Name == null || Candidate.Sex == null || Candidate.Birthday == null || Candidate.Phone == null || Candidate.Degree == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "會員資料不全",
                    AlertStatus = false,
                };
            }

            List<ResumeOpeningRecord> record = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == cajvm.resumeId && ror.OpeningId == cajvm.openingId).ToList();
            if (record.Count > 0)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "已有應徵紀錄",
                    AlertStatus = false
                };
            }

            var companyOpening = await _context.Openings.AsNoTracking().Include(o => o.Company).Where(o => o.OpeningId == cajvm.openingId).Select(o => new
            {
                OpeningId = o.OpeningId,
                OpeningTitle = o.Title,
                CompanyId = o.CompanyId,
                CompanyName = o.Company.CompanyName,
            }).FirstOrDefaultAsync();

            ResumeOpeningRecord recordResumeOpening = new ResumeOpeningRecord
            {
                ResumeId = cajvm.resumeId,
                OpeningId = companyOpening.OpeningId,
                OpeningTitle = companyOpening.OpeningTitle,
                CompanyId = companyOpening.CompanyId,
                CompanyName = companyOpening.CompanyName,
                ApplyDate = DateOnly.FromDateTime(DateTime.Now),
                InterviewYN = false,
                HireYN = false,
            };

            _context.ResumeOpeningRecords.Add(recordResumeOpening);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false
                };
            }

            return new ApplyJobOutputViewModel
            {
                AlertText = $"以 {resume.Title} 應徵 {companyOpening.CompanyName} 的 {companyOpening.OpeningTitle} 成功",
                AlertStatus = true,
            };
        }


        public IActionResult CompanyClassSelect()
        {
            var source = _context.CompanyCategories.Include(a => a.CompanyClasses);
            var temp = source.Select(b => new CompanyClassSelectViewModelcs
            {
                CompanyClassObj = b.CompanyClasses.Select(x => new { x.CompanyClassId, x.CompanyClassName}),
                CompanyCategoryId = b.CompanyCategoryId,
                CompanyCategoryName = b.CompanyCategoryName,
            });
            return Json(temp);
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
                var candidate = _context.Candidates
                    .FirstOrDefault(c => c.NationalId == candidateLogin.NationalId && c.Email == candidateLogin.Email);

                if (candidate != null && candidate.Password == candidateLogin.Password) // 假設密碼是明文儲存
                {
                    // 驗證通過，建立 claims，包含 CandidateId
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, candidate.CandidateId.ToString()),  // 存入 CandidateId
                new Claim(ClaimTypes.Name, candidateLogin.NationalId),                   // 使用身分證字號作為名稱
                new Claim(ClaimTypes.Role, "candidate")                                  // 設定角色為 candidate
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
                var company = _context.Companies
                    .FirstOrDefault(c => c.GUINumber == companyLogin.GUINumber);

                if (company != null && company.Password == companyLogin.Password) // 假設密碼是明文儲存
                {
                    // 驗證通過，建立 claims，包含 CompanyId
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, company.CompanyId.ToString()),  // 存入 CompanyId
                new Claim(ClaimTypes.Name, companyLogin.GUINumber),                   // 使用統一編號作為名稱
                new Claim(ClaimTypes.Role, "company")                                 // 設定角色為 company
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 執行登出操作，清除使用者登入資訊
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 重導向到登入頁面或首頁
            return RedirectToAction("Index", "Home"); 
        }
    }
}
