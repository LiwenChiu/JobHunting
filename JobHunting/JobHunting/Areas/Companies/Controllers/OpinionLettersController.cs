using JobHunting.Areas.Companies.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class OpinionLettersController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public OpinionLettersController(DuckCompaniesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
