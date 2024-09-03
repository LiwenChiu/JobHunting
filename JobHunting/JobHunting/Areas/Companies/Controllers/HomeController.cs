using Microsoft.AspNetCore.Mvc;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Candidates.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var Openings = _context.Openings;
            var TitleClasses= _context.TitleClasses;
            var ResumeOpeningRecords = _context.ResumeOpeningRecords;
            return View(Openings.Select(p=>new ReceiveResumeViewModel
            {
                Title = p.Title,
                TitleClassName = TitleClasses.Where(s=>s.TitleClassID==p.TitleClassID).Select(s=>s.TitleClassName).Single(),
                ApplyDate = ResumeOpeningRecords.Where(a=>a.OpeningID==p.OpeningID).Select(a=>a.ApplyDate).Single(),
            }));
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        //public List<ReceiveResumeViewModel> _ReceiveResume()
        //{
        //    var titleClasses = _context.TitleClasses;
        //    var resumeOpeningRecords = _context.ResumeOpeningRecords;
        //    var openings = _context.Openings;
        //    var query = (from TitleClass in _context.TitleClasses
        //                join Opening in _context.Openings
        //                on TitleClass.TitleClassID equals Opening.TitleClassID
        //                join ResumeOpeningRecord in _context.ResumeOpeningRecords
        //                on Opening.OpeningID equals ResumeOpeningRecord.OpeningID
        //                select new ReceiveResumeViewModel
        //                {
        //                    TitleClassName = TitleClass.TitleClassName,
        //                    Title = Opening.Title,
        //                    ApplyDate = ResumeOpeningRecord.ApplyDate
        //                }).Take(10);

        //    List<ReceiveResumeViewModel> result = query.ToList();

        //    return result;
        //}
    }
}
