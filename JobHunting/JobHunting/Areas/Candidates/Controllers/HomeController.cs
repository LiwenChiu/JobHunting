using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Companies.Models;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class HomeController : Controller
    {
        private readonly DuckCandidatesContext _context;


        public HomeController(DuckCandidatesContext context)
        {
            _context = context;
        }


        // GET: Candidates
        public IActionResult Index()
        {
            ViewBag._Record = _Record();
            ViewBag.B = _RecordB();

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

        public List<RecordViewmodel> _Record()
        {
            var resumeOpeningRecords = _context.ResumeOpeningRecords;
            var resumes = _context.Resumes;
            var openings = _context.Openings;

            var query = (from ResumeOpeningRecord in _context.ResumeOpeningRecords
                         join Resume in _context.Resumes
                         on ResumeOpeningRecord.ResumeID equals Resume.ResumeID
                         join Opening in _context.Openings
                         on ResumeOpeningRecord.OpeningID equals Opening.OpeningID
                         select new RecordViewmodel
                         {
                             ResumeOpeningRecordID = ResumeOpeningRecord.ResumeOpeningRecordID,
                             CompanyName = ResumeOpeningRecord.CompanyName,
                             OpeningTitle = ResumeOpeningRecord.OpeningTitle,
                             ApplyDate = ResumeOpeningRecord.ApplyDate,
                             Title = Resume.Title,

                         });

                     List<RecordViewmodel>result =query.ToList();

            return result;

        }
        public List<Models.ResumeOpeningRecord> _RecordB()
        {
             var result = _context.ResumeOpeningRecords.Take(10).ToList();
             return result;
        }



        
    }
}
