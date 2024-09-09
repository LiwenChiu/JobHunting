using Microsoft.AspNetCore.Mvc;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Candidates.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;
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
        public IActionResult Index()
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
        
        // GET: Companies/Home/IndexJson
        [HttpGet]
        public JsonResult IndexJson()
        {
            var Order = _context.CompanyOrders
                .Select(p => new
                {
                    OrderID = p.OrderId,
                    CompanyName = p.CompanyName,
                    GUINumber = p.GUINumber,
                    Title = p.Title,
                    Price = p.Price,
                    OrderDate = p.OrderDate,
                    Status = p.Status
                });

            return Json(Order);
        }

    }
}
