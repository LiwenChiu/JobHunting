using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using NuGet.Protocol;

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

        // POST: Candidates/Home/GetCandidateMemberData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<GetCandidateMemberDataViewModel> GetCandidateMemberData([FromBody] int id)
        {
            return _context.Candidates.AsNoTracking()
                .Where(cmd => cmd.CandidateId == id)
                .Select(cmd => new GetCandidateMemberDataViewModel
                {
                    Name = cmd.Name,
                    Headshot = cmd.Headshot,
                    TitleClass = cmd.TitleClass,
                    Email = cmd.Email,
                    Phone = cmd.Phone,
                    Address = cmd.Address,
                    EmploymentStatus = cmd.EmploymentStatus,
                }).Single();
        }

        // POST: Candidates/Home/GetCandidateMemberData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<GetCandidateResumesViewModel>> GetCandidateResumes([FromBody] int id)
        {
            return _context.Resumes
                .Where(r => r.CandidateId == id)
                .Select(r => new GetCandidateResumesViewModel
                {
                    ResumeId = r.ResumeId,
                    Title = r.Title,
                }).Take(2);
        }

        // POST: Candidates/Home/GetCandidateOpeningLikeRecords
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<GetCandidateOpeningLikeRecordsViewModel>> GetCandidateOpeningLikeRecords([FromBody] int id)
        {
            return _context.ResumeOpeningRecords.Include(ror => ror.Resume)
                .Where(ror => ror.Resume.CandidateId == id)
                .Select(ror => new GetCandidateOpeningLikeRecordsViewModel
                {
                    OpeningId = ror.OpeningId,
                    OpeningTitle = ror.OpeningTitle,
                }).Take(2);
        }

        // POST: Candidates/Home/ChangeTitleClass
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> ChangeTitleClass([FromBody][Bind("CandidateId,TitleClass")] ChangeTitleClassViewModel ctcvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["格式不正確", "失敗"];
                return returnStatus;
            }

            var candidate = await _context.Candidates.FindAsync(ctcvm.CandidateId);
            if (candidate == null)
            {
                returnStatus = ["錯誤", "失敗"];
                return returnStatus;
            }

            candidate.TitleClass = ctcvm.TitleClass;

            _context.Entry(candidate).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改職業失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = ["修改職業成功", "成功"];
            return returnStatus;
        }

        /*-------------------------------畫面顯示-------------------------------*/

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

        public IActionResult Notifications()
        {
            return View();
        }
        public IActionResult OpinionLetters()
        {
            return View();
        }





    }
}
