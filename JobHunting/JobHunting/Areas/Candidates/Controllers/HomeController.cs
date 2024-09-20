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
using Microsoft.AspNetCore.Authorization;
using JobHunting.Areas.Companies.ViewModel;
using System.Security.Claims;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Authorize(Roles = "candidate")]
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
        public async Task<GetCandidateMemberDataViewModel> GetCandidateMemberData()
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CandidateId))
            {
                return new GetCandidateMemberDataViewModel(); // 或處理未授權訪問的情況
            }
            return _context.Candidates.AsNoTracking()
                .Where(cmd => cmd.CandidateId.ToString() == CandidateId)
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
        public async Task<IEnumerable<GetCandidateResumesViewModel>> GetCandidateResumes()
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CandidateId))
            {
                return new List<GetCandidateResumesViewModel>(); // 或處理未授權訪問的情況
            }
            return _context.Resumes.AsNoTracking()
                .Where(r => r.CandidateId.ToString() == CandidateId)
                .Select(r => new GetCandidateResumesViewModel
                {
                    ResumeId = r.ResumeId,
                    Title = r.Title,
                    LastEditTime = r.LastEditTime,
                })
                .OrderByDescending(r => r.LastEditTime)
                .Take(2);
        }

        

        // POST: Candidates/Home/GetCandidateOpeningLikeRecords
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<GetCandidateOpeningLikeRecordsViewModel>> GetCandidateOpeningLikeRecords()
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CandidateId))
            {
                return new List<GetCandidateOpeningLikeRecordsViewModel>(); // 或處理未授權訪問的情況
            }

            var query = await _context.Candidates.Include(x => x.Openings)
                .FirstOrDefaultAsync(x => x.CandidateId.ToString() == CandidateId);

            if (query == null)
            {
                return new List<GetCandidateOpeningLikeRecordsViewModel>();
            }

            return query.Openings.Select(ror=>new GetCandidateOpeningLikeRecordsViewModel
            {
                OpeningId = ror.OpeningId,
                OpeningTitle = ror.Title,
                ResumeNumber = ror.ResumeNumber,
            })
            .OrderByDescending(r => r.ResumeNumber)
            .Take(2);
        }

        public async Task<FileResult> GetPicture(int CandidateId)
        {
            string noImageFilename = Path.Combine("StaticFiles", "images", "No_Image_Available.jpg");
            Candidate? candidate = await _context.Candidates.FindAsync(CandidateId);
            byte[] imageContent = candidate.Headshot != null ? candidate.Headshot : System.IO.File.ReadAllBytes(noImageFilename);
            return File(imageContent, "image/jpeg");
        }

        // POST: Candidates/Home/ChangeTitleClass
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> ChangeTitleClass([FromBody][Bind("TitleClass")] ChangeTitleClassViewModel ctcvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["格式不正確", "失敗"];
                return returnStatus;
            }
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                returnStatus = ["修改會員資料失敗", "失敗"];
                return returnStatus;
            }

            var candidate = await _context.Candidates.FindAsync(candidateId);
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
