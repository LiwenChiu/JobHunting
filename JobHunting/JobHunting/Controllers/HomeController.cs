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
        public IActionResult CompanyIndexList()
        {
            return Json(_context.Resumes.Include(c => c.Candidate).Select(c => new ResumeViewModel
            {
                ResumeID = c.ResumeId,
                CandidateID = c.CandidateId,
                Title = c.Title,
                TitleClassID = c.TitleClassId,
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
            }));

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
                TagID = p.TagId,
                TagClassID = p.TagClassId,
                TagName = p.TagName,
            }));
        }
        public async Task<IActionResult> TagClasses()
        {
            return Json(_context.TagClasses.Select(p => new
            {
                TagClassID = p.TagClassId,
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
