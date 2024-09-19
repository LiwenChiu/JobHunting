using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class AdminRecordsController : Controller
    {
        private readonly DuckCandidatesContext _context;

        public AdminRecordsController(DuckCandidatesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
