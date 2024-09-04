using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class C_ResumeController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public C_ResumeController(DuckCompaniesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResumeStorage()
        {
            return View();
        }
        public IActionResult ReceiveResume()
        {
            var Openings = _context.Openings;
            var TitleClasses = _context.TitleClasses;
            var ResumeOpeningRecords = _context.ResumeOpeningRecords;
            ViewBag.TitleClass = new SelectList(TitleClasses, "TitleClassID", "TitleClassName");
            return View(Openings.Select(p => new ReceiveResumeViewModel
            {
                Title = p.Title,
                TitleClassName = TitleClasses.Where(s => s.TitleClassID == p.TitleClassID).Select(s => s.TitleClassName).Single(),
                ApplyDate = ResumeOpeningRecords.Where(a => a.OpeningID == p.OpeningID).Select(a => a.ApplyDate).Single(),
            }));
        }
        [HttpGet]
        public IActionResult Opening(string id)
        {
            ViewBag.Opening = new SelectList(_context.Openings.Where(o => o.TitleClassID == id), "TitleClassID", "Title");
            return PartialView("_TitleClass");
        }
        //public IActionResult ResumeDetails(string id)
        //{
        //    return PartialView("_ResumeDetailsPartial", _context.OrderDetails.Where(od => Convert.ToString(od.OrderId) == id));
        //}
    }
    
}
