using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Candidates.Controllers
{
    public class HomeController : Controller
    {
        [Area("Candidates")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
