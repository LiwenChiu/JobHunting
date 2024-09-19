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
using System.Globalization;
using System.Net.Http;
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

        public async Task<OpeningsIndexOutputViewModel> OpeningsList(int id, int page,int count)
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
                return "��¾�ʤw����";
            }


            return "¾�ʤw���\����";
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
                return "��������¾�ʥ���";
            }

            return "��������¾�ʦ��\";
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
                    AlertText = "����",
                    AlertStatus = false,
                };
            }

            var Candidate = await _context.Candidates.FindAsync(cajvm.candidateId);
            if (Candidate == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "����",
                    AlertStatus = false,
                };
            }

            var resume = await _context.Resumes.FindAsync(cajvm.resumeId);
            if (resume == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "����",
                    AlertStatus = false,
                };
            }

            if (resume.ReleaseYN == false)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "�i�����}��",
                    AlertStatus = false,
                };
            }

            if (Candidate.Name == null || Candidate.Sex == null || Candidate.Birthday == null || Candidate.Phone == null || Candidate.Degree == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "�|����Ƥ���",
                    AlertStatus = false,
                };
            }

            List<ResumeOpeningRecord> record = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == cajvm.resumeId && ror.OpeningId == cajvm.openingId).ToList();
            if (record.Count > 0)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "�w�����x����",
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
                    AlertText = "����",
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
                    AlertText = "����",
                    AlertStatus = false
                };
            }

            return new ApplyJobOutputViewModel
            {
                AlertText = $"�H {resume.Title} ���x {companyOpening.CompanyName} �� {companyOpening.OpeningTitle} ���\",
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
                var candidate = _context.Candidates
                    .FirstOrDefault(c => c.NationalId == candidateLogin.NationalId && c.Email == candidateLogin.Email);

                if (candidate != null && candidate.Password == candidateLogin.Password) // ���]�K�X�O�����x�s
                {
                    // ���ҳq�L�A�إ� claims�A�]�t CandidateId
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, candidate.CandidateId.ToString()),  // �s�J CandidateId
                new Claim(ClaimTypes.Name, candidateLogin.NationalId),                   // �ϥΨ����Ҧr���@���W��
                new Claim(ClaimTypes.Role, "candidate")                                  // �]�w���⬰ candidate
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
                var company = _context.Companies
                    .FirstOrDefault(c => c.GUINumber == companyLogin.GUINumber);

                if (company != null && company.Password == companyLogin.Password) // ���]�K�X�O�����x�s
                {
                    // ���ҳq�L�A�إ� claims�A�]�t CompanyId
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, company.CompanyId.ToString()),  // �s�J CompanyId
                new Claim(ClaimTypes.Name, companyLogin.GUINumber),                   // �ϥβΤ@�s���@���W��
                new Claim(ClaimTypes.Role, "company")                                 // �]�w���⬰ company
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



        /* ------------------  �D¾�ݵ��U  ---------------------  */

        //POST : Home/AddCandidateRedgister
        [HttpPost]
        public async Task<IActionResult> AddCandidateRedgister([FromBody] CandidateRegisterVM cr)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "���U��ƥ���g���� or ����g���T" });
            }

            // �ˬd�q�l�l��Ψ����Ҹ��O�_�w�s�b
            if (await _context.Candidates.AnyAsync(c => c.NationalId == cr.NationalId || c.Email == cr.Email))
            {
                return Json(new { success = false, message = "���q�l�l��Ψ����Ҹ��w�Q���U" });
            }

            //// �K�X�[�K
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
                _logger.LogError(ex, "���U�L�{���o�Ϳ��~");
                return Json(new { success = false, message = "���U����", });
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
            if (!await ValidateGUINumber(cr.GUINumber , cr.CompanyName))
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
                    CompanyName =cr.CompanyName,
                    ContactName=cr.ContactName,
                    ContactPhone=cr.ContactPhone,
                    ContactEmail = cr.ContactEmail,
                    Status = cr.Status,
                    Date= cr.Date,
                    Address =cr.Address,
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


        //private async Task<bool> ValidateGUINumber(string GUINumber)
        //{
        //    try
        //    {
        //        string apiUrl = $"https://data.gcis.nat.gov.tw/od/data/api/9D17AE0D-09B5-4732-A8F4-81ADED04B679?$format=json&$filter=Business_Accounting_NO eq {GUINumber}";

        //        using (var httpClient = _httpClientFactory.CreateClient())
        //        {
        //            var response = await httpClient.GetAsync(apiUrl);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = await response.Content.ReadAsStringAsync();
        //                var options = new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                };
        //                var companies = JsonSerializer.Deserialize<List<CompanyInfoGUINumber>>(content, options);
        //                return companies != null && companies.Any();
        //            }
        //            else
        //            {
        //                Console.WriteLine("{response.StatusCode}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("{ex.Message}");
        //        return false;
        //    }
        //    return true;

        //}

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
            // ����n�X�ާ@�A�M���ϥΪ̵n�J��T
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // ���ɦV��n�J�����έ���
            return RedirectToAction("Index", "Home"); 
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AdminLogout()
        {
            // ����n�X�ާ@�A�M���ϥΪ̵n�J��T
            await HttpContext.SignOutAsync("AdminScheme");
   
            // ���ɦV��n�J�����έ���
            return RedirectToAction("Index", "Home", new { area = "Admins" });
        }


    }
}
