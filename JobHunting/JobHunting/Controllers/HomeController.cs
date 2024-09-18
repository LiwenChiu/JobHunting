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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // ����n�X�ާ@�A�M���ϥΪ̵n�J��T
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // ���ɦV��n�J�����έ���
            return RedirectToAction("Index", "Home"); 
        }
    }
}
