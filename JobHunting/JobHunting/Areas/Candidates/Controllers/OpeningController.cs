using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class OpeningController : Controller
    {
        private readonly DuckCandidatesContext _context;


        public OpeningController(DuckCandidatesContext context)
        {
            _context = context;
        }
        public IActionResult OpeningList()
        {
            return View();
        }
    }
}
