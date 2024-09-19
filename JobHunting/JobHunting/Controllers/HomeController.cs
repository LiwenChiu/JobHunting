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
                return "¶π¬æØ §w¶¨¬√";
            }


            return "¬æØ §w¶®•\¶¨¬√";
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
            catch (Exception ex)
            {

                return "®˙Æ¯¶¨¬√¬æØ •¢±—";
            }

            return "®˙Æ¯¶¨¬√¬æØ ¶®•\";
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
                    AlertText = "•¢±—",
                    AlertStatus = false,
                };
            }

            var Candidate = await _context.Candidates.FindAsync(cajvm.candidateId);
            if (Candidate == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "•¢±—",
                    AlertStatus = false,
                };
            }

            var resume = await _context.Resumes.FindAsync(cajvm.resumeId);
            if (resume == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "•¢±—",
                    AlertStatus = false,
                };
            }

            if (resume.ReleaseYN == false)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "ºiæ˙•º∂}©Ò",
                    AlertStatus = false,
                };
            }

            if (Candidate.Name == null || Candidate.Sex == null || Candidate.Birthday == null || Candidate.Phone == null || Candidate.Degree == null)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "∑|≠˚∏ÍÆ∆§£•˛",
                    AlertStatus = false,
                };
            }

            List<ResumeOpeningRecord> record = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == cajvm.resumeId && ror.OpeningId == cajvm.openingId).ToList();
            if (record.Count > 0)
            {
                return new ApplyJobOutputViewModel
                {
                    AlertText = "§w¶≥¿≥ºx¨ˆø˝",
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



                    AlertText = "•¢±—",

                    AlertStatus = false
                };
            }

            return new ApplyJobOutputViewModel
            {
                AlertText = $"•H {resume.Title} ¿≥ºx {companyOpening.CompanyName} ™∫ {companyOpening.OpeningTitle} ¶®•\",
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




            return "∑sºW´H•Û¶®•\";

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





                // ®D¬æ™Ã≈Á√“≈ﬁøË
                var candidate = _context.Candidates
                    .FirstOrDefault(c => c.NationalId == candidateLogin.NationalId && c.Email == candidateLogin.Email);

                if (candidate != null && candidate.Password == candidateLogin.Password) // ∞≤≥]±KΩX¨O©˙§Â¿x¶s
                {
                    // ≈Á√“≥qπL°A´ÿ•ﬂ claims°A•]ßt CandidateId
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, candidate.CandidateId.ToString()),  // ¶s§J CandidateId
                new Claim(ClaimTypes.Name, candidateLogin.NationalId),                   // ®œ•Œ®≠§¿√“¶r∏πß@¨∞¶W∫Ÿ
                new Claim(ClaimTypes.Role, "candidate")                                  // ≥]©w®§¶‚¨∞ candidate

            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);




                    // ®œ•Œ Cookie ª{√“∂i¶Êµn§J
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, message = "®D¬æ™Ãµn§J¶®•\", role = "candidate" });
                }
                else
                {
                    return Json(new { success = false, message = "®D¬æ™Ãµn§J•¢±—°G±b∏π©Œ±KΩXø˘ª~" });

                }
            }
            else if (loginRequest.Role == "company")
            {
                var companyLogin = loginRequest.CompanyLoginVM;



                // §Ω•q≈Á√“≈ﬁøË
                var company = _context.Companies
                    .FirstOrDefault(c => c.GUINumber == companyLogin.GUINumber);

                if (company != null && company.Password == companyLogin.Password) // ∞≤≥]±KΩX¨O©˙§Â¿x¶s
                {
                    // ≈Á√“≥qπL°A´ÿ•ﬂ claims°A•]ßt CompanyId
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, company.CompanyId.ToString()),  // ¶s§J CompanyId
                new Claim(ClaimTypes.Name, companyLogin.GUINumber),                   // ®œ•Œ≤Œ§@Ωs∏πß@¨∞¶W∫Ÿ
                new Claim(ClaimTypes.Role, "company")                                 // ≥]©w®§¶‚¨∞ company

            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);




                    //POST : Home/AddCandidateRedgister

                    // ®œ•Œ Cookie ª{√“∂i¶Êµn§J
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, message = "§Ω•qµn§J¶®•\", role = "company" });
                }
                else
                {
                    return Json(new { success = false, message = "§Ω•qµn§J•¢±—°G≤Œ§@Ωs∏π©Œ±KΩXø˘ª~" });
                }
            }

            return Json(new { success = false, message = "µLÆƒ™∫®§¶‚" });
        }


        [HttpPost]
        //public async Task<IActionResult> AdminDoLogin([FromBody] AdminLoginInputModel loginRequest)
        //{
        //    // ∫ﬁ≤z™Ã≈Á√“≈ﬁøË
        //    var admin = _context.Admins
        //        .FirstOrDefault(a => a.PersonnelCode == loginRequest.PersonnelCode);

        //    if (admin != null && admin.Password == loginRequest.Password) // ∞≤≥]±KΩX¨O©˙§Â¿x¶s
        //    {

        //        return Json(new
        //        {
        //            success = false,
        //            message = "Ë®ªÂ?Ë≥áÊ??™Â°´ÂØ´Â???or ?™Â°´ÂØ´Ê≠£Á¢? });
        //        }

        //    // Ê™¢Êü•?ªÂ??µ‰ª∂?ñË∫´‰ªΩË??üÊòØ?¶Â∑≤Â≠òÂú®
        //    if (await _context.Candidates.AnyAsync(c => c.NationalId == cr.NationalId || c.Email == cr.Email))
        //        {
        //            return Json(new { success = false, message = "Ê≠§ÈõªÂ≠êÈÉµ‰ª∂Ê?Ë∫´‰ªΩË≠âË?Â∑≤Ë¢´Ë®ªÂ?" });
        //        }

        //        //// ÂØÜÁ¢º?†Â?
        //        //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(cr.Password);

        //        try
        //        {
        //            Candidate inster = new Candidate
        //            {
        //                NationalId = cr.NationalId,
        //                Email = cr.Email,
        //                Password = cr.Password,

        //            };


        //            using (var transaction = await _context.Database.BeginTransactionAsync())
        //            {
        //                try
        //                {
        //                    _context.Candidates.Add(inster);
        //                    await _context.SaveChangesAsync();
        //                    await transaction.CommitAsync();
        //                }
        //                catch
        //                {
        //                    await transaction.RollbackAsync();
        //                    throw;
        //                }
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Ë®ªÂ??éÁ?‰∏≠Áôº?üÈåØË™?);
        //            return Json(new { success = false, message = "Ë®ªÂ?Â§±Ê?", });
        //        }

        //        return Json(new
        //        {
        //            success = true,
        //            message = "Â∑≤Ë®ª?äÊ???, });
        //        }


        /* ------------------  ?¨Âè∏Á´ØË®ª?? ---------------------  */
        //}
        //POST : Home/AddCompanyRedgister
        //[HttpPost]
        //public async Task<IActionResult> AddCompanyRedgister([FromBody] CompanyRegisterVM cr)
        //{

        //    // ≈Á√“≥qπL°A´ÿ•ﬂ claims°A•]ßt AdminId
        //    var claims = new List<Claim>

        //{
        //    new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),  // ¶s§J AdminId
        //    new Claim(ClaimTypes.Name, admin.PersonnelCode.ToString()),       // ®œ•Œ§u∏πß@¨∞¶W∫Ÿ
        //    new Claim(ClaimTypes.Role, "admin")                              // ≥]©w®§¶‚¨∞ admin
        //};

        //    var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");

        //    // ®œ•Œ Cookie ª{√“∂i¶Êµn§J
        //    await HttpContext.SignInAsync("AdminScheme", new ClaimsPrincipal(claimsIdentity));

        //    return Json(new { success = true, message = "∫ﬁ≤z™Ãµn§J¶®•\", role = "admin" });
        //}
        //    else
        //    {
        //        return Json(new { success = false, message = "∫ﬁ≤z™Ãµn§J•¢±—°G§u∏π©Œ±KΩXø˘ª~" });
        //    }
        //}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {



            // ∞ı¶Êµn•Xæﬁß@°A≤M∞£®œ•Œ™Ãµn§J∏Í∞T
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // ≠´æ…¶V®Ïµn§J≠∂≠±©Œ≠∫≠∂
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AdminLogout()
        {
            // ∞ı¶Êµn•Xæﬁß@°A≤M∞£®œ•Œ™Ãµn§J∏Í∞T
            await HttpContext.SignOutAsync("AdminScheme");

            // ≠´æ…¶V®Ïµn§J≠∂≠±©Œ≠∫≠∂
            return RedirectToAction("Index", "Home", new { area = "Admins" });
        }



    }
}
