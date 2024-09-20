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
using Microsoft.Data.SqlClient;
using System.Text.Json;
namespace JobHunting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DuckContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(ILogger<HomeController> logger, DuckContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<OpeningsIndexOutputViewModel> OpeningsList(int id, int page, int count)
        {
            var openings = _context.Openings.AsNoTracking().Include(a => a.Company).Include(o => o.Candidates).Select(b => new OpeningsIndexViewModel
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
            });

            var openingIndexOutput = new OpeningsIndexOutputViewModel
            {
                totalDataCount = openings.Count(),
                OpeningsIndexOutput = openings.Skip((page - 1) * count).Take(count),
            };

            return openingIndexOutput;
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
                    RequiredNumber = o.RequiredNumber,
                    ResumeNumber = o.ResumeNumber,
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

            if (companyOpening == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false
                };
            }

            var opening = await _context.Openings.FindAsync(cajvm.openingId);
            opening.ResumeNumber++;

            _context.Entry(opening).State = EntityState.Modified;

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
                CompanyClassObj = b.CompanyClasses.Select(x => new { x.CompanyClassId, x.CompanyClassName }),
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

        [HttpPost]
        public async Task<IActionResult> AdminDoLogin([FromBody] AdminLoginInputModel loginRequest)
        {
            // 管理者驗證邏輯
            var admin = _context.Admins
                .FirstOrDefault(a => a.PersonnelCode == loginRequest.PersonnelCode);

            if (admin != null && admin.Password == loginRequest.Password) // 假設密碼是明文儲存
            {
                // 驗證通過，建立 claims，包含 AdminId
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),  // 存入 AdminId
            new Claim(ClaimTypes.Name, admin.PersonnelCode.ToString()),       // 使用工號作為名稱
            new Claim(ClaimTypes.Role, "admin")                              // 設定角色為 admin
        };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");

                // 使用 Cookie 認證進行登入
                await HttpContext.SignInAsync("AdminScheme", new ClaimsPrincipal(claimsIdentity));

                return Json(new { success = true, message = "管理者登入成功", role = "admin" });
            }
            else
            {
                return Json(new { success = false, message = "管理者登入失敗：工號或密碼錯誤" });
            }
        }


        /* ------------------  求職端註冊  ---------------------  */

        //POST : Home/AddCandidateRedgister
        [HttpPost]
        public async Task<IActionResult> AddCandidateRedgister([FromBody] CandidateRegisterVM cr)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "註冊資料未填寫完成 or 未填寫正確" });
            }

            // 檢查電子郵件或身份證號是否已存在
            if (await _context.Candidates.AnyAsync(c => c.NationalId == cr.NationalId))
            {
                return Json(new { success = false, message = "此身份證號已被註冊過" });
            }

            if (await _context.Candidates.AnyAsync(c => c.Email == cr.Email))
            {
                return Json(new { success = false, message = "此電子郵件已被註冊過" });
            }

            //// 密碼加密
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(cr.Password);

            try
            {
                Candidate inster = new Candidate
                {
                    NationalId = cr.NationalId,
                    Email = cr.Email,
                    Password = cr.Password,

                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Candidates.Add(inster);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊過程中發生錯誤");
                return Json(new { success = false, message = "註冊失敗", });
            }

            return Json(new { success = true, message = "已註冊成功", });
        }


        /* ------------------  公司端註冊  ---------------------  */

        //POST : Home/AddCompanyRedgister
        [HttpPost]
        public async Task<IActionResult> AddCompanyRedgister([FromBody] CompanyRegisterVM cr)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "註冊資料未填寫完成 or 未填寫正確" });
            }

            // 驗證統一編號
            if (!await ValidateGUINumber(cr.GUINumber, cr.CompanyName))
            {
                return Json(new { success = false, message = "統一編號 or 公司名稱輸入錯誤" });
            }

            if (await _context.Companies.AnyAsync(c => c.GUINumber == cr.GUINumber))
            {
                return Json(new { success = false, message = "此統一編號已被註冊過" });
            }


            //// 密碼加密
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(cr.Password);

            try
            {
                Company inster = new Company
                {
                    GUINumber = cr.GUINumber,
                    CompanyName = cr.CompanyName,
                    ContactName = cr.ContactName,
                    ContactPhone = cr.ContactPhone,
                    ContactEmail = cr.ContactEmail,
                    Status = cr.Status,
                    Date = cr.Date,
                    Address = cr.Address,
                    Password = cr.Password,

                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Companies.Add(inster);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊過程中發生錯誤");
                return Json(new { success = false, message = "註冊失敗", });
            }

            return Json(new { success = true, message = "已註冊完成，目前已進入「審核」，待審核完畢會再通知結果。 ", });
        }


        // 公司行號營業項目代碼 api ， 判斷參數是否跟api裡的資料相同
        public async Task<bool> ValidateGUINumber(string GUINumber, string CompanyName)
        {
            try
            {
                string apiUrl = $"https://data.gcis.nat.gov.tw/od/data/api/9D17AE0D-09B5-4732-A8F4-81ADED04B679?$format=json&$filter=Business_Accounting_NO eq {GUINumber}";

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var companies = JsonSerializer.Deserialize<List<CompanyInfoGUINumber>>(content, options);

                        //確保公司名稱與傳回結果中的公司名稱相符
                        if (companies != null && companies.Any())
                        {
                            return companies.Any(c => c.Company_Name.Equals(CompanyName, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AdminLogout()
        {
            // 執行登出操作，清除使用者登入資訊
            await HttpContext.SignOutAsync("AdminScheme");
   
            // 重導向到登入頁面或首頁
            return RedirectToAction("Index", "Home", new { area = "Admins" });
        }


    }
}
