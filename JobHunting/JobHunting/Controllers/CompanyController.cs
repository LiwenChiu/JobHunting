using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace JobHunting.Controllers
{
    public class CompanyController : Controller
    {
        private readonly DuckContext _context;
        public CompanyController(DuckContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CompanyIndex()
        {
            return View();
        }
        public async Task<IActionResult> ResumeIntro()
        {
            return PartialView();
        }

        public async Task<IActionResult> CompanyIndexList()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return Json(_context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Select(c => new CompanyResumeListViewModel
            {
                ResumeID = c.ResumeId,
                CandidateID = c.CandidateId,
                Title = c.Title,
                Intro = c.Intro,
                Autobiography = c.Autobiography,
                WorkExperience = c.WorkExperience,
                Certification = c.Certification,
                Time = c.Time,
                WishAddress = c.Address,
                Name = c.Candidate.Name,
                Sex = c.Candidate.Sex,
                Degree = c.Candidate.Degree,
                Address = c.Candidate.Address,
                TagObj = c.Tags.Select(z => new { z.TagId, z.TagName }),
                Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0
            }));
        }
        public async Task<IEnumerable<ResumesOutput>> SelectIndexList([FromBody] ResumeInputModel resume)
        {
            EditResume(resume);
            var today = DateOnly.FromDateTime(DateTime.Now);
            var source = _context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).ToList();
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
                    b.Address.Substring(4, 3) == resume.Area ||
                    (b.Address.Substring(4, 3) == resume.Area && b.Address.Substring(0, 3) == resume.zipCode) ||
                    //b.Degree.Contains(resume.Edu) ||
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
                return temp;
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
                    b.Sex == resume.Sex ||
                    b.Age.ToString().Contains(resume.serchText) ||
                    b.Address.Contains(resume.serchText.Replace("臺", "台")) ||
                    b.Address.Substring(4, 3) == resume.Area ||
                    resume.serchText.Any(c => b.Name.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0) ||  //不區分英文字母大小寫，逐一檢查
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
                return temp;
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
