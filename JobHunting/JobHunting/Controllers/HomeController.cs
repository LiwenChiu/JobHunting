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
            return Json(_context.Candidates.Select(c => new Candidate
            {
                Name = c.Name,
                Sex = c.Sex,
                Birthday = c.Birthday,
                Degree = c.Degree,
                Address = c.Address,
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
        public async Task<IActionResult> TagList()
        {
            return Json(_context.Tags.Include(p => p.TagClass).Select(p => new ResumeViewModel
            {
                TagName = p.TagName,
                TagClassName = p.TagClass.TagClassName,
            }));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
