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
using JobHunting.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using Microsoft.CodeAnalysis.Scripting;
using JobHunting.Areas.Candidates.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace JobHunting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DuckContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EmailService _emailserver;
        public HomeController(ILogger<HomeController> logger, DuckContext context, IHttpClientFactory httpClientFactory, EmailService emailserver)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _emailserver = emailserver;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("進入首頁");
            return View();
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        public IActionResult CandidateResetPassword()
        {
            return View();
        }
        public IActionResult CompanyResetPassword()
        {
            return View();
        }
        public IActionResult ResendVerificationLetter()
        {
            return View();
        }




        public async Task<OpeningsIndexOutputViewModel> OpeningsList(int page, int count)
        {
            var candidateIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (candidateIdClaim == null)
            {
                var openingsUnLogin = _context.Openings.AsNoTracking().Include(a => a.Company).Select(b => new OpeningsIndexViewModel
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
                    LikeYN = null,
                });

                var openingUnLoginDataOutput = new OpeningsIndexOutputViewModel
                {
                    totalDataCount = openingsUnLogin.Count(),
                    OpeningsIndexOutput = openingsUnLogin.Skip((page - 1) * count).Take(count),
                };

                return openingUnLoginDataOutput;
            }
            var candidateId = int.Parse(candidateIdClaim.Value);

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
                LikeYN = b.Candidates.Where(c => c.CandidateId == candidateId).FirstOrDefault() != null,
            });

            var openingIndexOutput = new OpeningsIndexOutputViewModel
            {
                totalDataCount = openings.Count(),
                OpeningsIndexOutput = openings.Skip((page - 1) * count).Take(count),
            };

            return openingIndexOutput;
        }

        [Authorize]
        [HttpPost]
        public async Task<string> AddFavorite([FromBody] AddFavoriteOpeningsViewModel favorite)
        {
            try
            {
                var candidateIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (candidateIdClaim == null)
                {
                    return "無法獲取使用者 ID，請重新登入";
                }

                var candidateId = int.Parse(candidateIdClaim.Value);

                var query = "INSERT INTO CandidateOpeningLikeRecords(CandidateId,OpeningId) VALUES (@CandidateId ,@OpeningId)";
                var candidateIdParam = new SqlParameter("@CandidateId", candidateId);
                var openingIdParam = new SqlParameter("@OpeningId", favorite.OpeningId);
                await _context.Database.ExecuteSqlRawAsync(query, candidateIdParam, openingIdParam);
            }
            catch (Exception ex)
            {
                return "此職缺已收藏";
            }


            return "職缺已成功收藏";
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> DeleteFavorite([FromBody] DeleteFavoriteOpeningsViewModel dfovm)
        {
            try
            {
                var candidateIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (candidateIdClaim == null)
                {
                    return "無法獲取使用者 ID，請重新登入";
                }

                // 確保 CandidateId 是從認證資料獲取
                var candidateId = int.Parse(candidateIdClaim.Value);
                var query = "DELETE FROM CandidateOpeningLikeRecords WHERE CandidateId = @CandidateId AND OpeningId = @OpeningId";
                var candidateIdParam = new SqlParameter("@CandidateId", candidateId);
                var openingIdParam = new SqlParameter("@OpeningId", dfovm.OpeningId);
                await _context.Database.ExecuteSqlRawAsync(query, candidateIdParam, openingIdParam);
            }
            catch (Exception ex)
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

        [Authorize]
        //GET: Home/ResumesJson
        public async Task<IEnumerable<ResumesOutputViewModel>> ResumesJson()
        {
            var candidateIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (candidateIdClaim == null)
            {

                return Enumerable.Empty<ResumesOutputViewModel>();
            }
            var candidateId = int.Parse(candidateIdClaim.Value);
            var resumes = _context.Resumes.AsNoTracking()
                .Where(r => r.CandidateId == candidateId)
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
        public async Task<ApplyJobOutputViewModel> ApplyJob([FromBody][Bind("resumeId,openingId")] ApplyJobViewModel cajvm)
        {
            var candidateIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (candidateIdClaim == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "未找到 CandidateId，請重新登入",
                    AlertStatus = false,
                };
            }

            // 轉換 CandidateId
            var candidateId = int.Parse(candidateIdClaim.Value);

            var Candidate = await _context.Candidates.FindAsync(candidateId);
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


        public IActionResult TitleClassSelect()
        {
            var source = _context.TitleCategories.Include(a => a.TitleClasses);
            var temp = source.Select(b => new TitleClassSelectViewModelcs
            {
                TitleClassObj = b.TitleClasses.Select(x => new { x.TitleClassId, x.TitleClassName }),
                TitleCategoryId = b.TitleCategoryId,
                TitleCategoryName = b.TitleCategoryName,
            });
            return Json(temp);
        }
        [Authorize]
        [HttpPost]
        public async Task<string> AddLetter([FromForm] InsterLetter letter)
        {
            // 提取 NameIdentifier 和 Role，動態判斷是 candidate 還是 company
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
            {
                return "無法確認用戶身份，請重新登入";
            }

            OpinionLetter opinionLetter = new OpinionLetter();

            if (userRole == "company")
            {
                opinionLetter.CompanyId = Convert.ToInt32(userId);
            }
            else if (userRole == "candidate")
            {
                opinionLetter.CandidateId = Convert.ToInt32(userId);
            }

            opinionLetter.Class = letter.Letterclass;
            opinionLetter.SubjectLine = letter.SubjectLine;
            opinionLetter.Content = letter.Content;
            //opinionLetter.SendTime = letter.SendTime;
            var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            opinionLetter.SendTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);
            IsPicture(letter, opinionLetter);  // 根據你的邏輯處理圖片
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

                // 判斷輸入是 Email 還是 NationalId
                var candidate = _context.Candidates
                    .FirstOrDefault(c =>
                        (c.NationalId == candidateLogin.CandidateLogin || c.Email == candidateLogin.CandidateLogin));

                if (candidate == null)
                {
                    return Json(new { success = false, message = "無此求職者，請檢查身分證字號或電子郵件" });
                }

                if (!candidate.VerifyEmailYN)
                {
                    return Json(new { success = false, message = "求職者尚未驗證電子郵件" });
                }

                if (BCrypt.Net.BCrypt.Verify(candidateLogin.Password, candidate.Password))
                {
                    // 驗證通過，建立 claims，包含 CandidateId
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, candidate.CandidateId.ToString()),  // 存入 CandidateId
                new Claim(ClaimTypes.Name, candidateLogin.CandidateLogin),              // 使用 NationalId 或 Email 作為名稱
                new Claim(ClaimTypes.Role, "candidate")                                  // 設定角色為 candidate
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 使用 Cookie 認證進行登入
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, message = "求職者登入成功", role = "candidate" });
                }
                else
                {
                    return Json(new { success = false, message = "求職者登入失敗：密碼錯誤" });
                }
            }
            else if (loginRequest.Role == "company")
            {
                var companyLogin = loginRequest.CompanyLoginVM;

                // 公司驗證邏輯
                var company = _context.Companies
                    .FirstOrDefault(c => c.GUINumber == companyLogin.GUINumber);
                if(company != null)
                {
                    if (!company.Status)
                    {
                        return Json(new { success = false, message = "公司帳號尚未審核通過" });
                    }
                    if (BCrypt.Net.BCrypt.Verify(companyLogin.Password, company.Password))
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

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // 使用 Cookie 認證進行登入
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

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

            if (cr.Password != cr.ConfirmPassword)
            {
                return Json(new { success = false, message = "密碼與確認密碼不一致。" });
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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(cr.Password);

            try
            {
                Candidate inster = new Candidate
                {
                    NationalId = cr.NationalId,
                    Email = cr.Email,
                    Password = hashedPassword,
                    VerifyEmailYN = cr.VerifyEmailYN,
                    RegisterTime = cr.RegisterTime,

                };


                _context.Candidates.Add(inster);
                await _context.SaveChangesAsync();
                //string verificationUrl = _emailserver.GenerateVerificationToken(cr.Email);
                _emailserver.SendEmail(cr.Email, $"您已使用{cr.Email} 註冊'小鴨上工'的會員成功");
                return Json(new { success = true, message = "您已註冊會員完成，'小鴨上工歡迎您','請務必前往您的信箱查閱驗證信件'", });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊過程中發生錯誤");
                return Json(new { success = false, message = "註冊失敗", });
            }

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

            if (cr.Password != cr.ConfirmPassword)
            {
                return Json(new { success = false, message = "密碼與確認密碼不一致。" });
            }

            // 驗證統一編號
            if (!await ValidateGUINumber(cr.GUINumber, cr.CompanyName))
            {
                return Json(new { success = false, message = "統一編號 or 公司名稱需與商工登記資料相符" });
            }

            if (await _context.Companies.AnyAsync(c => c.GUINumber == cr.GUINumber))
            {
                return Json(new { success = false, message = "此統一編號已被註冊過" });
            }


            //// 密碼加密
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(cr.Password);

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
                    Password = hashedPassword,

                };


                _context.Companies.Add(inster);
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊過程中發生錯誤");
                return Json(new { success = false, message = "註冊失敗。資料未填寫完成 or 未填寫正確", });
            }

            return Json(new { success = true, message = "已註冊完成。目前已進入「審核」，待審核完畢會再通知結果。 ", });
        }


        // 公司行號營業項目代碼 api ， 判斷參數是否跟api裡的資料相同
        public async Task<bool> ValidateGUINumber(string GUINumber, string CompanyName)
        {
            try
            {
                string apiUrl = $"https://data.gcis.nat.gov.tw/od/data/api/9D17AE0D-09B5-4732-A8F4-81ADED04B679?$format=json&$filter=Business_Accounting_NO eq {GUINumber}";

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    //向 API 發送 GET 請求，並等待回應
                    var response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions
                        {
                            //忽略大小寫
                            PropertyNameCaseInsensitive = true
                        };
                        var companies = JsonSerializer.Deserialize<List<CompanyInfoGUINumber>>(content, options);

                        //確認 API 返回的公司名稱是否與提供的 CompanyName 相符（忽略大小寫）
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

        // 求職者驗證電子郵件方法 在_emailserver.SendEmail時會調用此方法，在驗證連結的部分
        [HttpGet]
        public IActionResult VerifyEmail(string token, string email, long expiry)
        {
            string Token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email) || expiry < DateTime.UtcNow.Ticks)
            {
                return BadRequest("驗證連結無效或已過期。");
            }

            string Email = Encoding.UTF8.GetString(Convert.FromBase64String(email));

            // 找到對應的求職者mail
            var candidate = _context.Candidates.FirstOrDefault(c => c.Email == Email);
            if (candidate == null)
            {
                return BadRequest("無效的驗證連結。");
            }

            if (candidate.VerifyEmailYN)
            {
                return RedirectToAction("Login", "Home"); // 已驗證，跳轉到登入頁面
            }

            // 更新 VerifyEmailYN 為 true
            candidate.VerifyEmailYN = true;
            _context.SaveChanges();


            // 跳轉到登入頁面
            return RedirectToAction("Login", "Home");

        }
        //管理端審核通知信的跳轉登入畫面
        [HttpGet]
        public IActionResult VerifyStatusEmail()
        {

            // 跳轉到登入頁面
            return RedirectToAction("Login", "Home");
        }



        //驗證信件重新發送
        [HttpPost]
        public async Task<IActionResult> SendVerificationLetter([FromBody]VerificationLetterViewmodel svl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "資料未填寫完整" });
                }


                var candidateYemail = _context.Candidates.FirstOrDefault(c => c.Email == svl.Email);
                if (candidateYemail != null)
                {
                    if (candidateYemail.VerifyEmailYN)
                    {
                        return Json(new { success = false, message = "此帳號已驗證過", });
                    }
                }else if(candidateYemail == null)
                {
                    return Json(new { success = false, message = "此電子信箱未註冊過。", });
                }
                
            
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "資料未填寫正確", });
            }

            //string verificationUrl = _emailserver.GenerateVerificationToken(svl.Email);
            _emailserver.SendEmail(svl.Email, $"您已使用{svl.Email} 註冊'小鴨上工'的會員成功");
            return Json(new { success = true, message = "以重新發送驗證信'請務必前往您的信箱查閱信件'", });
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AdminLogout()
        {
            // 執行登出操作，清除使用者登入資訊
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 重導向到登入頁面或首頁
            return RedirectToAction("AdminLogin", "Home");
        }

        [Authorize]
        //GET: Home/GetCandidateData
        public async Task<GetCandidateDataOutputViewModel> GetCandidateData()
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return new GetCandidateDataOutputViewModel { AlertText = "失敗" };
            }

            var candidateData = await _context.Candidates.AsNoTracking()
                .Where(c => c.CandidateId == candidateId)
                .Select(c => new GetCandidateDataInputViewModel
                {
                    Name = c.Name,
                    Sex = c.Sex,
                    Birthday = c.Birthday,
                    Phone = c.Phone,
                    Address = c.Address,
                    Degree = c.Degree,
                    VerifyEmailYN = c.VerifyEmailYN,
                }).FirstOrDefaultAsync();

            if (candidateData == null)
            {
                return new GetCandidateDataOutputViewModel { AlertText = "失敗" };
            }

            if (!candidateData.VerifyEmailYN)
            {
                return new GetCandidateDataOutputViewModel { DataStatus = false, AlertText = "驗證信箱" };
            }

            if (string.IsNullOrEmpty(candidateData.Name) || !candidateData.Sex.HasValue || !candidateData.Birthday.HasValue || string.IsNullOrEmpty(candidateData.Phone) || string.IsNullOrEmpty(candidateData.Address) || string.IsNullOrEmpty(candidateData.Degree))
            {
                return new GetCandidateDataOutputViewModel { DataStatus = false, AlertText = "完整填寫會員資料" };
            }

            return new GetCandidateDataOutputViewModel { DataStatus = true, AlertText = "請選擇履歷" };
        }

        public async Task<string> GetRole()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                if (role == "admin")
                {
                    return "admin";
                }

                if (role == "candidate")
                {
                    return "candidate";
                }

                if (role == "company")
                {
                    return "company";
                }
            }

            return "";
        }
        public async Task<OpeningSelectOutputViewModel> SelectOpeningsList([FromBody] OpeningSelectInputViewModel opening)
        {
            var candidateIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            EditResume(opening);
            if (candidateIdClaim == null)
            {
                var sourceUnlogin = _context.Openings.AsNoTracking().Include(a => a.Company).Include(a => a.Candidates).Include(x => x.Tags).Include(y => y.TitleClasses).Where(y => y.ReleaseYN == true)
                        .Select(c => new
                        {
                            OpeningId = c.OpeningId,
                            CompanyId = c.CompanyId,
                            Title = c.Title,
                            Address = c.Address,
                            Description = c.Description,
                            Degree = c.Degree,
                            Benefits = c.Benefits,
                            SalaryMax = c.SalaryMax,
                            SalaryMin = c.SalaryMin,
                            Time = c.Time,
                            ContactEmail = c.ContactEmail,
                            ContactName = c.ContactName,
                            ContactPhone = c.ContactPhone,
                            CompanyName = c.Company.CompanyName,
                            TitleClass = c.TitleClasses.Select(x => new { x.TitleClassId, x.TitleClassName }),
                            LikeYN = false,
                        });
                if (!opening.SearchText.IsNullOrEmpty())
                {
                    sourceUnlogin = sourceUnlogin.Where(b =>
                             b.CompanyName.Contains(opening.SearchText) ||
                             b.Benefits.Contains(opening.SearchText) ||
                             b.Description.Contains(opening.SearchText) ||
                             b.Title.Contains(opening.SearchText) ||
                             b.TitleClass.Any(x => x.TitleClassName.Contains(opening.SearchText)) ||
                             b.Time.Contains(opening.SearchText) ||
                             b.Address.Contains(opening.SearchText));
                }
                if (!opening.AreaName.IsNullOrEmpty())
                {
                    sourceUnlogin = sourceUnlogin.Where(b =>
                             b.Address.Contains(opening.AreaName));
                }
                if (opening.ClassNumber != null)
                {
                    sourceUnlogin = sourceUnlogin.Where(b =>
                             b.TitleClass.Any(x => x.TitleClassId == opening.ClassNumber));
                }
                if (opening.Salary != null)
                {
                    sourceUnlogin = sourceUnlogin.Where(b =>
                             opening.Salary <= b.SalaryMin ||
                             b.SalaryMax >= opening.Salary );
                }
                if (opening.SearchText == "" && opening.AreaName == "" && opening.ClassNumber == null && opening.Salary == null)
                {
                    sourceUnlogin = sourceUnlogin;
                }
                var temp = sourceUnlogin.Select(c => new OpeningSelectViewModel
                {
                    OpeningId = c.OpeningId,
                    CompanyId = c.CompanyId,
                    Title = c.Title,
                    Address = c.Address,
                    Description = c.Description,
                    Degree = c.Degree,
                    Benefits = c.Benefits,
                    SalaryMax = c.SalaryMax,
                    SalaryMin = c.SalaryMin,
                    Time = c.Time,
                    ContactEmail = c.ContactEmail,
                    ContactName = c.ContactName,
                    ContactPhone = c.ContactPhone,
                    CompanyName = c.CompanyName,
                    TitleClass = c.TitleClass,
                    LikeYN = null,
                });
                var openingSelectOutput = new OpeningSelectOutputViewModel();
                openingSelectOutput.totalDataCount = temp.Count();
                openingSelectOutput.OpeningsIndexOutput = temp.Skip((opening.Page - 1) * opening.Count).Take(opening.Count);
                return openingSelectOutput;
            }
            
            else
            {
                var candidateId = int.Parse(candidateIdClaim.Value);
                var sourcelogin = _context.Openings.AsNoTracking().Include(a => a.Company).Include(a => a.Candidates).Include(x => x.Tags).Where(y => y.ReleaseYN == true)
                        .Select(c => new
                        {
                            OpeningId = c.OpeningId,
                            CompanyId = c.CompanyId,
                            Title = c.Title,
                            Address = c.Address,
                            Description = c.Description,
                            Degree = c.Degree,
                            Benefits = c.Benefits,
                            SalaryMax = c.SalaryMax,
                            SalaryMin = c.SalaryMin,
                            Time = c.Time,
                            ContactEmail = c.ContactEmail,
                            ContactName = c.ContactName,
                            ContactPhone = c.ContactPhone,
                            CompanyName = c.Company.CompanyName,
                            TitleClass = c.TitleClasses.Select(x => new { x.TitleClassId, x.TitleClassName }),
                            LikeYN = c.Candidates.Where(c => c.CandidateId == candidateId).FirstOrDefault() != null,
                        });
                if (!opening.SearchText.IsNullOrEmpty())
                {
                    sourcelogin = sourcelogin.Where(b =>
                             b.CompanyName.Contains(opening.SearchText) ||
                             b.Benefits.Contains(opening.SearchText) ||
                             b.Description.Contains(opening.SearchText) ||
                             b.Title.Contains(opening.SearchText) ||
                             b.TitleClass.Any(x => x.TitleClassName.Contains(opening.SearchText)) ||
                             b.Time.Contains(opening.SearchText) ||
                             b.Address.Contains(opening.SearchText));
                }
                if (!opening.AreaName.IsNullOrEmpty())
                {
                    sourcelogin = sourcelogin.Where(b =>
                             b.Address.Contains(opening.AreaName));
                }
                if (opening.ClassNumber != null)
                {
                    sourcelogin = sourcelogin.Where(b =>
                             b.TitleClass.Any(x => x.TitleClassId == opening.ClassNumber));
                }
                if (opening.Salary != null)
                {
                    sourcelogin = sourcelogin.Where(b =>
                             opening.Salary <= b.SalaryMin ||
                             b.SalaryMax >= opening.Salary);
                }
                if (opening.SearchText == "" && opening.AreaName == "" && opening.ClassNumber == null && opening.Salary == null)
                {
                    sourcelogin = sourcelogin;
                }
                var temp = sourcelogin.Select(c => new OpeningSelectViewModel
                {
                    OpeningId = c.OpeningId,
                    CompanyId = c.CompanyId,
                    Title = c.Title,
                    Address = c.Address,
                    Description = c.Description,
                    Degree = c.Degree,
                    Benefits = c.Benefits,
                    SalaryMax = c.SalaryMax,
                    SalaryMin = c.SalaryMin,
                    Time = c.Time,
                    ContactEmail = c.ContactEmail,
                    ContactName = c.ContactName,
                    ContactPhone = c.ContactPhone,
                    CompanyName = c.CompanyName,
                    TitleClass = c.TitleClass,
                    LikeYN = c.LikeYN,
                });
                var openingSelectOutput = new OpeningSelectOutputViewModel();
                openingSelectOutput.totalDataCount = temp.Count();
                openingSelectOutput.OpeningsIndexOutput = temp.Skip((opening.Page - 1) * opening.Count).Take(opening.Count);
                return openingSelectOutput;
            }
        }
        public string NormalizeAddress(string address)
        {
            return address.Replace("臺", "台");
        }
        public void EditResume(OpeningSelectInputViewModel opening)
        {
            opening.AreaName = NormalizeAddress(opening.AreaName);
        }


    }
}
