using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace JobHunting.Controllers
{
    public class CompanyController : Controller
    {
        private readonly DuckContext _context; IWebHostEnvironment _hostingEnvironment;
        public CompanyController(DuckContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CompanyIndex()
        {
            return View();
        }
        public async Task<IActionResult> ResumeDetail(int resumeID,int companyID)
        {
            if (resumeID == null)
            {
                return NotFound();
            }
            var today = DateOnly.FromDateTime(DateTime.Now);
            return Json(_context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Include(y => y.TitleClasses).Include(z => z.Companies)
                .Where(y => y.ResumeId == resumeID)
                .Select(c => new ResumesIntroViewModel
                {
                    ResumeId = c.ResumeId,
                    CandidateId = c.CandidateId,
                    Title = c.Title,
                    Intro = c.Intro,
                    Autobiography = c.Autobiography,
                    WorkExperience = c.WorkExperience,
                    WishAddress = c.Address,
                    WishTime = c.Time,
                    Name = c.Candidate.Name,
                    Phone = c.Candidate.Phone,
                    Email = c.Candidate.Email,
                    EmploymentStatus = c.Candidate.EmploymentStatus,
                    MilitaryService = c.Candidate.MilitaryService,
                    Sex = c.Candidate.Sex,
                    Degree = c.Candidate.Degree,
                    Address = c.Candidate.Address,
                    TagObj = c.Tags.Select(z => new { z.TagId, z.TagName }),
                    TitleObj = c.TitleClasses.Select(z => new { z.TitleClassId, z.TitleClassName}),
                    Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0,
                    LikeYN = c.Companies.Where(x => x.CompanyId == companyID).FirstOrDefault() != null,
                }));
        }
        public async Task<IActionResult> ResumeIntro(int? candidateId, int? resumeId)
        {
            if (resumeId == null)
            {
                return NotFound();
            }
            else
            {
                var resume = await _context.Resumes.Include(a => a.Candidate).Where(c => c.ResumeId == resumeId && c.ReleaseYN == true).Select(c => new ResumeIDViewModel
                {
                    ResumeId = c.ResumeId,
                    CandidateId = c.CandidateId,
                    Title = c.Title,
                }).FirstOrDefaultAsync(m => m.CandidateId == candidateId);
                return View(resume);
            }
           
        }
        public async Task<CompanyResumeListViewModel> CompanyIndexList([FromBody] ResumeInputModel resume)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var source = _context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Where(b => b.ReleaseYN == true);
            var temp = source.Select(c => new CompanyResumes
            {
                ResumeID = c.ResumeId,
                CandidateID = c.CandidateId,
                Intro = c.Intro,
                Autobiography = c.Autobiography,
                WorkExperience = c.WorkExperience,
                WishAddress = c.Address,
                Name = c.Candidate.Name,
                Sex = c.Candidate.Sex,
                Degree = c.Candidate.Degree,
                Address = c.Candidate.Address,
                TagObj = c.Tags.Select(z => new { z.TagId, z.TagName }),
                Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0
            });
            var resumesSelectOutput = new CompanyResumeListViewModel
            {
                TotalDataCount = temp.Count(),
                ResumeOutput = temp.Skip((resume.CurrentPage - 1) * resume.Perpage).Take(resume.Perpage),
            };
            return resumesSelectOutput;
        }
        public async Task<ResumesSelectOutput> SelectIndexList([FromBody] ResumeInputModel resume)
        {
            EditResume(resume);
            var today = DateOnly.FromDateTime(DateTime.Now);
            var source = _context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Where(b => b.ReleaseYN == true);
            if (resume.serchText.IsNullOrEmpty())
            {
                var temp = source.Select(c => new
                {
                    ResumeID = c.ResumeId,
                    CandidateID = c.CandidateId,
                    Title = c.Title,
                    Intro = c.Intro,
                    Autobiography = c.Autobiography,
                    WorkExperience = c.WorkExperience,
                    WishAddress = c.Address,
                    Name = c.Candidate.Name,
                    Sex = c.Candidate.Sex,
                    Birthday = c.Candidate.Birthday,
                    Degree = c.Candidate.Degree,
                    Address = c.Candidate.Address,
                    skill = c.Tags.Select(z => new { z.TagId, z.TagName }),
                    Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0
                }).Where(b =>
                    b.Address.Substring(0, 3) == resume.Area ||
                    b.Degree == resume.Edu ||
                    b.skill.Any(z => z.TagId == resume.Skill)
                ).Select(x => new ResumesOutput
                {
                    ResumeID = x.ResumeID,
                    CandidateID = x.CandidateID,
                    Title = x.Title,
                    Intro = x.Intro,
                    Autobiography = x.Autobiography,
                    Address = x.Address,
                    Name = x.Name,
                    Sex = x.Sex,
                    Age = x.Age,
                    WishAddress = x.WishAddress,
                    Degree = x.Degree,
                    TagObj = x.skill
                });
                var resumesSelectOutput = new ResumesSelectOutput
                {
                    TotalDataCount = temp.Count(),
                    ResumeOutput = temp.Skip((resume.CurrentPage - 1) * resume.Perpage).Take(resume.Perpage),
                };
                return resumesSelectOutput;
            }
            else
            {
                var temp = source.Select(c => new
                {
                    ResumeID = c.ResumeId,
                    CandidateID = c.CandidateId,
                    Title = c.Title,
                    Intro = c.Intro,
                    Autobiography = c.Autobiography,
                    WorkExperience = c.WorkExperience,
                    Certification = c.Certification,
                    WishAddress = c.Address,
                    Time = c.Time,
                    Name = c.Candidate.Name,
                    Sex = c.Candidate.Sex,
                    Birthday = c.Candidate.Birthday,
                    Degree = c.Candidate.Degree,
                    Address = c.Candidate.Address,
                    skill = c.Tags.Select(z => new { z.TagId, z.TagName }),
                    Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0
                }).Where(b =>
                    resume.serchText.Any(c => b.Name.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0) ||  //不區分英文字母大小寫，逐一檢查
                    b.Sex == resume.Sex ||
                    b.Age.ToString().Contains(resume.serchText) ||
                    b.Address.Contains(resume.serchText) ||
                    b.Address.Substring(0, 3) == resume.Area ||
                    b.Degree == resume.Edu ||
                    b.Degree.Contains(resume.serchText) ||
                    b.skill.Any(z => z.TagId == resume.Skill)
                ).Select(x => new ResumesOutput
                {
                    ResumeID = x.ResumeID,
                    CandidateID = x.CandidateID,
                    Title = x.Title,
                    Intro = x.Intro,
                    Autobiography = x.Autobiography,
                    WorkExperience = x.WorkExperience,
                    Certification = x.Certification,
                    Time = x.Time,
                    Address = x.Address,
                    Name = x.Name,
                    Sex = x.Sex,
                    Age = x.Age,
                    WishAddress = x.WishAddress,
                    Degree = x.Degree,
                    TagObj = x.skill
                });
                var resumesSelectOutput = new ResumesSelectOutput
                {
                    TotalDataCount = temp.Count(),
                    ResumeOutput = temp.Skip((resume.CurrentPage - 1) * resume.Perpage).Take(resume.Perpage),
                };
                return resumesSelectOutput;
            }

        }
        public async Task<IActionResult> TagClasses()
        {
            return Json(_context.TagClasses.Include(a => a.Tags).Select(p => new TagSelectList
            {
                TagClassId = p.TagClassId,
                TagClassName = p.TagClassName,
                TagObj = p.Tags.Select(z => new { z.TagId, z.TagName })
            }));
        }
        public async Task<IActionResult> GetOpenings(int id)
        {
            return Json(_context.Openings.Include(a => a.Company).Where(c => c.CompanyId == id).Select(p => new OpeningsListViewModel
            {
                OpeningId = p.OpeningId,
                CompanyId = p.CompanyId,
                Title = p.Title
            }));
        }
        [HttpPost]
        public async Task<string> AddInterview([FromBody]AddInterviewViewModel letter)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            Notification notificationLetter = new Notification();
            notificationLetter.CompanyId = letter.CompanyId;
            notificationLetter.CandidateId = Convert.ToInt32(letter.CandidateId);
            notificationLetter.OpeningId = letter.OpeningId;
            notificationLetter.ResumeId = Convert.ToInt32(letter.ResumeId);
            notificationLetter.Status = letter.Status;
            notificationLetter.SubjectLine = letter.SubjectLine;
            notificationLetter.Content = letter.Content;
            notificationLetter.Address = letter.Address;
            notificationLetter.SendDate = today;
            notificationLetter.AppointmentDate = letter.AppointmentDate;
            notificationLetter.AppointmentTime = letter.AppointmentTime;
            _context.Notifications.Add(notificationLetter);
            await _context.SaveChangesAsync();
            return "發送面試成功";
        }
        [HttpPost]
        public async Task<string> AddFavorite([FromBody] AddFavoriteResumeViewModel favorite)
        {
            try
            {
                var query = "INSERT INTO CompanyResumeLikeRecords(CompanyId,ResumeId) VALUES (@CompanyId ,@ResumeId)";
                var companyIdParam = new SqlParameter("@CompanyId", Convert.ToInt32(favorite.CompanyId));
                var resumeIdParam = new SqlParameter("@ResumeId", Convert.ToInt32(favorite.ResumeId));
                await _context.Database.ExecuteSqlRawAsync(query, companyIdParam, resumeIdParam);
            }
            catch (Exception ex)
            {
                return "此履歷已收藏";
            }


            return "履歷已成功收藏";
        }
        public async Task<string> DeleteFavorite([FromBody] DeleteFavoriteResumesViewModel df)
        {
            try
            {
                var query = "DELETE FROM CompanyResumeLikeRecords WHERE CompanyId = @CompanyId AND ResumeId = @ResumeId";
                var companyIdParam = new SqlParameter("@CompanyId", df.CompanyId);
                var resumeIdParam = new SqlParameter("@ResumeId", df.ResumeId);
                await _context.Database.ExecuteSqlRawAsync(query, companyIdParam, resumeIdParam);
            }
            catch (Exception ex)
            {
                return "取消收藏履歷失敗";
            }

            return "取消收藏履歷成功";
        }
        [NonAction]
        static int CalculateAge(DateOnly birthday, DateOnly today)
        {
            int age = today.Year - birthday.Year;
            if (today < new DateOnly(today.Year, birthday.Month, birthday.Day))
            {
                age--;
            }
            return age;
        }
        public async Task<FileResult> GetPicture(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string FileName = Path.Combine(webRootPath, "images", "No_Image_Available.jpg");
            Resume c = await _context.Resumes.FindAsync(id);
            byte[] ImageContent = c?.Headshot != null ? c.Headshot : System.IO.File.ReadAllBytes(FileName);
            return File(ImageContent, "image/jpeg");
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
        public string NormalizeAddress(string address)
        {
            return address.Replace("臺", "台");
        }
        public void EditResume(ResumeInputModel resume)
        {
            resume.Area = NormalizeAddress(resume.Area);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
