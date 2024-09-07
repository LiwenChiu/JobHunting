using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        public async Task<IEnumerable<RecordOutputmodel>> filter([FromBody] RecordViewmodel rv)
        {

           return  _context.ResumeOpeningRecords
                 .Include(c => c.Resume).Include(a => a.Opening).ToList()
                 .Where(rvfilter => rvfilter.ResumeOpeningRecordID == rv.ResumeOpeningRecordID ||
                           rvfilter.CompanyName.Contains(rv.CompanyName) ||
                           rvfilter.ApplyDate.ToString().Contains(rv.ApplyDate) ||
                           rvfilter.OpeningTitle.Contains(rv.OpeningTitle) ||
                           rvfilter.Resume.Title.Contains(rv.Title))
                 .Select(p => new RecordOutputmodel
                 {
                     ResumeOpeningRecordID = p.ResumeOpeningRecordID,
                     ApplyDate = p.ApplyDate,
                     CompanyName = p.CompanyName,
                     OpeningTitle = p.OpeningTitle,
                     Title = p.Resume.Title,
                 });
        }


        [HttpPost]
        public async Task<IEnumerable<OpeningDetail_Outputmodel>> OpeningDetail([FromBody] OpeningDetail_inputmodel OP)
        {
            return _context.Openings.Include(i => i.Company)
                    .Where(opi => opi.CompanyID == OP.CompanyID ||
                           opi.OpeningID == OP.OpeningID)
                    .Select(p => new OpeningDetail_Outputmodel
                    {
                        CompanyName = p.Company.CompanyName,
                        OpeningID = p.OpeningID,
                        CompanyID = p.CompanyID,
                        Title = p.Title,
                        Address = p.Address,
                        ContactPhone = p.ContactPhone,
                        ContactEmail = p.ContactEmail,
                        Time = p.Time,
                        SalaryMax = p.SalaryMax,
                        SalaryMin = p.SalaryMin,
                        Description = p.Description,
                        Benefits = p.Benefits,
                    });
        }
    }






}
