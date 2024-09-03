using Microsoft.AspNetCore.Mvc;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Candidates.Models;
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
        public IActionResult Index()
        {
            //todo: 步驟1.大家定義自己的function跟ViewBag名稱
            // 透過function取得自己所需的View Model資料
            ViewBag._ReceiveResume = _ReceiveResume();

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
        public List<ReceiveResumeViewModel> _ReceiveResume()
        {
            var titleClasses = _context.TitleClasses;
            var resumeOpeningRecords = _context.ResumeOpeningRecords;
            var openings = _context.Openings;
            var query = (from TitleClass in _context.TitleClasses
                        join Opening in _context.Openings
                        on TitleClass.TitleClassID equals Opening.TitleClassID
                        join ResumeOpeningRecord in _context.ResumeOpeningRecords
                        on Opening.OpeningID equals ResumeOpeningRecord.OpeningID
                        select new ReceiveResumeViewModel
                        {
                            TitleClassName = TitleClass.TitleClassName,
                            Title = Opening.Title,
                            ApplyDate = ResumeOpeningRecord.ApplyDate
                        }).Take(10);

            List<ReceiveResumeViewModel> result = query.ToList();

            return result;
        }
    }
}
