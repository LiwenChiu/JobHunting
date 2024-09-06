using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class RecordController : Controller
    {
        private readonly DuckCandidatesContext _context;


        public RecordController(DuckCandidatesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RecordHistory()
        {
            return View();
        }

        /*-------------------------------------------------*/

        // POST: Candidates/Home/Applyresult
        [HttpPost]
        public JsonResult Recorresult()
        {

            return Json(_context.ResumeOpeningRecords
                 .Include(c => c.Resume).Include(a => a.Opening).Select(p => new
                 {
                     ResumeOpeningRecordID = p.ResumeOpeningRecordID,
                     ApplyDate = p.ApplyDate,
                     CompanyName = p.CompanyName,
                     OpeningTitle = p.OpeningTitle,
                     Title = p.Resume.Title,
                 }));
        }




        //Post: Candidates/Record/Recordfilter
        [HttpPost]
        public async Task<IEnumerable<RecordViewmodel>> filter([FromBody] RecordViewmodel rv)
        {

            return (_context.ResumeOpeningRecords
                 .Include(c => c.Resume).Include(a => a.Opening)
                 .Where(rvfilter => rvfilter.ResumeOpeningRecordID == rv.ResumeOpeningRecordID ||
                           rvfilter.CompanyName.Contains(rv.CompanyName) ||
                           rvfilter.ApplyDate==rv.ApplyDate ||
                           rvfilter.OpeningTitle.Contains(rv.OpeningTitle) ||
                           rvfilter.Resume.Title.Contains(rv.Title))
                 .Select(p => new RecordViewmodel
                 {
                     ResumeOpeningRecordID = p.ResumeOpeningRecordID,
                     ApplyDate = p.ApplyDate,
                     CompanyName = p.CompanyName,
                     OpeningTitle = p.OpeningTitle,
                     Title = p.Resume.Title,

                 }));

             

        }

    }






}
