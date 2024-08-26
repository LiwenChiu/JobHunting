using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Companies.Controllers
{
    public class HomeController : Controller
    {
        [Area("Companies")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
