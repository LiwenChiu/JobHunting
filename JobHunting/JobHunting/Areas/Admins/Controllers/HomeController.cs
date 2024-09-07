using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemberManagement()
        {
            return View();
        }
        
        public IActionResult ClientServiceCenter()
        {
            return View();
        }
        //GET:Admins/Home/IndexJson_opinionletter
        [HttpGet]
        public JsonResult IndexJson_opinionletter()
        {
            var opinionletter = _context.OpinionLetters.Select(p => new
            {
                Class= p.Class,
                SubjectLine= p.SubjectLine,
                Status= p.Status,
            });
            return Json(opinionletter);
        }
        public IActionResult PricingPlansManagement()
        {
            return View();
        }

        public IActionResult TagManagement()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TagManagement([Bind("TagClassId,TagClass,TagId,TagName")]TagManagementViewModel tmvm)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("TagManagement");
            }
            return View(tmvm);
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
