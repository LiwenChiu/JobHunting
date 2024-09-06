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

        public IActionResult CompanyList()
        {
            return View();
        }
        //GET:Admins/Home/IndexJson
        [HttpGet]
        public JsonResult IndexJson() 
        {
            var companyList = _context.Companies.Select(p => new
            {
                CompanyID = p.CompanyID,
                CompanyName = p.CompanyName,
                GUINumber = p.GUINumber,
                ContactName = p.ContactName,
                ContactPhone = p.ContactPhone,
                ContactEmail = p.ContactEmail
            });
            return Json(companyList);
        }
        public IActionResult CandidateList()
        {
            return View();
        }
        //GET:Admins/Home/IndexJson_candidate
        [HttpGet]
        public JsonResult IndexJson_candidate()
        {
            var candidateList = _context.Candidates.Select(p => new
            {
                CandidateID= p.CandidateID,
                Name= p.Name,
                Sex= p.Sex,
                Birthday= p.Birthday,
                Degree= p.Degree,
                Address= p.Address,
            });
            return Json(candidateList);
        }
        public IActionResult ClientServiceCenter()
        {
            return View();
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
        public IActionResult TagManagement([Bind("TagClassID,TagClass,TagID,TagName")]TagManagementViewModel tmvm)
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
