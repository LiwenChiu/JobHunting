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

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Member()
        {
            return View();
        }

        public IActionResult Record()
        {
            var TitleClasses = _context.TitleClasses;
            var Openings = _context.Openings;

            return View(_context.ResumeOpeningRecords
                 .Include(c => c.Resume).Include(a => a.Opening).Select(p => new RecordViewmodel
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
