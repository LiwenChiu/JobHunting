using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "admin")]    
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult MemberManagement()
        {
            return View();
        }
       
        public IActionResult Login()
        {
            return View();
        }

         

    }
}
