using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
        private readonly DuckCandidatesContext _context;

        public HomeController(DuckCandidatesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemberManagement()
        {
            return View();
        }
        
        public IActionResult ClientServiceCenter()
        {
            return View();
        }
       

        public IActionResult Login()
        {
            return View();
        }
    }
}
