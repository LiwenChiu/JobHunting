using Azure.Core;
using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
        [HttpPost]
        public async Task<IEnumerable<ResumesOutput>> SelectIndexList([FromBody] ResumeInputModel resume)
        {
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
                    b.Address.Substring(3, 4) == resume.Area ||
                    //b.Degree.Contains(resume.Edu) ||
                    b.skill.Any(z => z.TagId == resume.Skill))
                                   .Select(x => new ResumesOutput
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
                                b.Age.ToString().Contains(resume.serchText) ||
                                b.Address.Contains(resume.serchText) ||
                                resume.serchText.Any(c => b.Name.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0 ||  //不區分英文字母大小寫，逐一檢查
                                b.skill.Any(z => z.TagId == resume.Skill)))
                                    .Select(x => new ResumesOutput
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
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CompanyIndex()
        {
            return View();
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
        [HttpPost]
        public async Task<string> AddLetter([FromBody] InsterLetter letter)
        {
            OpinionLetter opinionLetter = new OpinionLetter();
            opinionLetter.CompanyId = letter.CompanyId;
            opinionLetter.Class = letter.Letterclass;
            opinionLetter.SubjectLine = letter.SubjectLine;
            opinionLetter.Content = letter.Content;
            _context.OpinionLetters.Add(opinionLetter);
            await _context.SaveChangesAsync();
            return "新增信件成功";
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
