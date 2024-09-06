using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        [HttpPost]
        public async Task<IEnumerable<ResumeViewModel>> CompanyIndexList([FromBody] ResumeViewModel resume)
        {
            return _context.Resumes.Include(a => a.Candidate).Select(c => new ResumeViewModel
            {
                ResumeID = c.ResumeID,
                CandidateID = c.CandidateID,
                Title = c.Title,
                TitleClassID = c.TitleClassID,
                Intro = c.Intro,
                Autobiography = c.Autobiography,
                WorkExperience = c.WorkExperience,
                Certification = c.Certification,
                Time = c.Time,
                WishAddress = c.Address,
                Name = c.Candidate.Name,
                Sex = c.Candidate.Sex,
                Birthday = c.Candidate.Birthday,
                Degree = c.Candidate.Degree,
                Address = c.Candidate.Address,
            }).Where(b =>
                    b.Sex == resume.Sex ||
                    b.Name.Contains(resume.Name) ||
                    b.Address.Contains(resume.Address) ||
                    b.Degree.Contains(resume.Degree)
                ).Select(c => new ResumeViewModel
                {
                    ResumeID = c.ResumeID,
                    CandidateID = c.CandidateID,
                    Title = c.Title,
                    TitleClassID = c.TitleClassID,
                    Intro = c.Intro,
                    Autobiography = c.Autobiography,
                    WorkExperience = c.WorkExperience,
                    Certification = c.Certification,
                    Time = c.Time,
                    WishAddress = c.Address,
                    Name = c.Name,
                    Sex = c.Sex,
                    Birthday = c.Birthday,
                    Degree = c.Degree,
                    Address = c.Address,
                });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CompanyIndex()
        {
            return View();
        }
        public async Task<IActionResult> Tags()
        {
            return Json(_context.Tags.Select(p => new 
            {
                TagID = p.TagID,
                TagClassID = p.TagClassID,
                TagName = p.TagName,
            }));
        }
        public async Task<IActionResult> TagClasses()
        {
            return Json(_context.TagClasses.Select(p => new
            {
                TagClassID = p.TagClassID,
                TagClassName = p.TagClassName,
            }));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
