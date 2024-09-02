using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class HomeController : Controller
    {
        public IActionResult Index()
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

    }
}
