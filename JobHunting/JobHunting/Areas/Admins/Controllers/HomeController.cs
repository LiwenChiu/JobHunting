using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Security.Claims;

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
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            Console.WriteLine(User.FindFirst(ClaimTypes.Role)?.Value);
            return View();
        }
        [Authorize(Roles = "admin")]
        public IActionResult MemberManagement()
        {
            return View();
        }
       

         

    }
}
