using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JobHunting.Controllers
{
    public class CandidateController : Controller
    {
        private readonly DuckContext _context; IWebHostEnvironment _hostingEnvironment;
        public CandidateController(DuckContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> OpeningDetail(int? companyId, int? openingId)
        {
            if (openingId == null)
            {
                return NotFound();
            }
            else
            {
                var opening = await _context.Openings.Include(a => a.Company).Where(c => c.OpeningId == openingId && c.ReleaseYN == true).Select(c => new OpeningIDViewModel
                {
                    OpeningId = c.OpeningId,
                    CompanyId = c.CompanyId,
                    Title = c.Title,
                }).FirstOrDefaultAsync(m => m.CompanyId == companyId);
                return View(opening);
            }

        }
        public async Task<IActionResult> OpeningList(int openingID)
        {
            if (openingID == null)
            {
                return NotFound();
            }
            else
            {
                var candidateIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (candidateIdClaim == null)
                {
                    return Json(_context.Openings.Include(a => a.Company).Include(x => x.Tags).Include(y => y.TitleClasses).Include(z => z.Candidates)
                        .Where(y => y.OpeningId == openingID && y.ReleaseYN == true)
                        .Select(b => new OpeningListDetailViewModel
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
                            TitleClassName = b.TitleClasses.Select(tc => tc.TitleClassName),
                            TagName = b.Tags.Select(t => t.TagName),
                            LikeYN = null,
                        }));
                }

                int candidateID = int.Parse(candidateIdClaim.Value);

                return Json(_context.Openings.Include(a => a.Company).Include(x => x.Tags).Include(y => y.TitleClasses).Include(z => z.Candidates)
                .Where(y => y.OpeningId == openingID && y.ReleaseYN == true)
                .Select(b => new OpeningListDetailViewModel
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
                    TitleClassName = b.TitleClasses.Select(tc => tc.TitleClassName),
                    TagName = b.Tags.Select(t => t.TagName),
                    LikeYN = b.Candidates.Where(x => x.CandidateId == candidateID).FirstOrDefault() != null,
                }));
            }
            
        }
        public async Task<IActionResult> GetResumes()
        {
            var candidateIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (candidateIdClaim == null)
            {
                return Unauthorized("無法獲取求職者資訊，請重新登入");
            }

            int candidateID = int.Parse(candidateIdClaim.Value);
            var resumes = _context.Resumes.Include(a => a.Candidate).Where(c => c.CandidateId == candidateID);
            if(!resumes.IsNullOrEmpty())
            {
                return Json(resumes.Select(p => new ResumesListViewModel
                {
                    CandidateId = p.CandidateId,
                    ResumesId = p.ResumeId,
                    Title = p.Title,
                }));
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Candidates" });
            }
            //return Json(_context.Resumes.Include(a => a.Candidate).Where(c => c.CandidateId == candidateID).Select(p => new ResumesListViewModel
            //{
            //    CandidateId = p.CandidateId,
            //    ResumesId = p.ResumeId,
            //    Title = p.Title,
            //    ResumeIsExist = p.ResumeId != null ? true : false,
            //}));
        }
        [HttpPost]
        public async Task<string> AddApply([FromBody] AddApplyViewModel letter)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            ResumeOpeningRecord applyLetter = new ResumeOpeningRecord();
            applyLetter.CompanyId = letter.CompanyId;
            applyLetter.ResumeId = letter.ResumeId;
            applyLetter.OpeningId = letter.OpeningId;
            applyLetter.CompanyName = letter.CompanyName;
            applyLetter.OpeningTitle = letter.OpeningTitle;
            applyLetter.ApplyDate = today;
            _context.ResumeOpeningRecords.Add(applyLetter);
            await _context.SaveChangesAsync();
            return "發送應徵職缺成功";
        }
        public async Task<FileResult> GetPicture(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string FileName = Path.Combine(webRootPath, "images", "No_Image_Available.jpg");
            Company c = await _context.Companies.FindAsync(id);
            byte[] ImageContent = c?.Picture != null ? c.Picture : System.IO.File.ReadAllBytes(FileName);
            return File(ImageContent, "image/jpeg");
        }
        [HttpPost]
        public async Task<string> AddFavorite([FromBody] AddFavoriteOpeningsViewModel favorite)
        {
            try
            {
                var candidateIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (candidateIdClaim == null)
                {
                    return "請重新登入";
                }

                int candidateID = int.Parse(candidateIdClaim.Value);

                var query = "INSERT INTO CandidateOpeningLikeRecords(CandidateId,OpeningId) VALUES (@CandidateId ,@OpeningId)";
                var CandidateIdParam = new SqlParameter("@CandidateId", candidateID);
                var OpeningIdParam = new SqlParameter("@OpeningId", favorite.OpeningId);
                await _context.Database.ExecuteSqlRawAsync(query, CandidateIdParam, OpeningIdParam);
            }
            catch (Exception ex)
            {
                return "此職缺已收藏";
            }


            return "職缺已成功收藏";
        }
        public async Task<string> DeleteFavorite([FromBody] DeleteFavoriteOpeningsViewModel df)
        {
            try
            {
                var candidateIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (candidateIdClaim == null)
                {
                    return "請重新登入";
                }

                int candidateID = int.Parse(candidateIdClaim.Value);

                var query = "DELETE FROM  CandidateOpeningLikeRecords WHERE CandidateId = @CandidateId AND OpeningId = @OpeningId";
                var CandidateIdParam = new SqlParameter("@CandidateId", candidateID);
                var OpeningIdParam = new SqlParameter("@OpeningId", df.OpeningId);
                await _context.Database.ExecuteSqlRawAsync(query, CandidateIdParam, OpeningIdParam);
            }
            catch (Exception ex)
            {
                return "取消職缺失敗";
            }

            return "取消收藏職缺成功";
        }
    }
}
