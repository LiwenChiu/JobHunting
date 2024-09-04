using Microsoft.AspNetCore.Mvc;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Candidates.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class HomeController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public HomeController(DuckCompaniesContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Member()
        {
            return View();
        }
        public IActionResult Intro()
        {
            return View();
        }
        public IActionResult Opening()
        {
            return View();
        }
        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Message()
        {
            return View();
        }
        public IActionResult Notifications()
        {
            return View();
        }
        public IActionResult OpinionLetters()
        {
            return View();
        }
        public IActionResult PricingPlans()
        {
            return View();
        }
        public IActionResult PricingOrderHistory()
        {
            return View();
        }
    }
}
